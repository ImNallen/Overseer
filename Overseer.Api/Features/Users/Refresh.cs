using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Abstractions.Time;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Services.Authentication;

namespace Overseer.Api.Features.Users;

public record RefreshRequest(string RefreshToken);

public record RefreshResponse(string AccessToken, int ExpiresIn, string RefreshToken, int RefreshExpiresIn);

public record RefreshCommand(string RefreshToken) : ICommand<RefreshResponse>;

public class RefreshEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/users/refresh", async (
            RefreshRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new RefreshCommand(request.RefreshToken);

            Result<RefreshResponse> result = await sender.Send(command, cancellationToken);

            return result.IsFailure ? CustomResults.Problem(result) : Results.Ok(result.Value);
        })
        .WithTags(Tags.Users)
        .AllowAnonymous();
}

public class RefreshValidator : AbstractValidator<RefreshCommand>
{
    public RefreshValidator() => RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required.");
}

public class RefreshHandler : ICommandHandler<RefreshCommand, RefreshResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RefreshHandler(
        IUnitOfWork unitOfWork,
        IJwtTokenGenerator jwtTokenGenerator,
        IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<RefreshResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        User? user = await _unitOfWork.Users
            .Where(u => u.RefreshToken == request.RefreshToken)
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<RefreshResponse>(UserErrors.InvalidCredentials);
        }

        if (user.RefreshTokenExpires < _dateTimeProvider.UtcNow)
        {
            return Result.Failure<RefreshResponse>(UserErrors.RefreshTokenExpired);
        }

        Token token = _jwtTokenGenerator.GenerateToken(user);

        user.SetRefreshToken(token.RefreshToken, _dateTimeProvider.UtcNow.AddMinutes(token.RefreshExpiresIn));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshResponse(
            token.AccessToken,
            token.ExpiresIn,
            token.RefreshToken,
            token.RefreshExpiresIn);
    }
}
