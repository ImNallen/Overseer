using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Abstractions.Encryption;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Abstractions.Time;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Services.Authentication;

namespace Overseer.Api.Features.Users;

public record LoginRequest(string Username, string Password);

public record TokenResponse(string AccessToken, int ExpiresIn, string RefreshToken, int RefreshExpiresIn);

public record LoginCommand(string Username, string Password) : ICommand<Token>;

public class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/users/login", async (
            LoginRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginCommand(request.Username, request.Password);

            Result<Token> result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }

            return Results.Ok(new TokenResponse(
                result.Value.AccessToken,
                result.Value.ExpiresIn,
                result.Value.RefreshToken,
                result.Value.RefreshExpiresIn));
        })
        .WithTags(Tags.Users)
        .AllowAnonymous();
}

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}

public class LoginHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<LoginCommand, Token>
{
    public async Task<Result<Token>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user = await unitOfWork.Users
            .Where(x => (string)x.Username == request.Username)
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<Token>(UserErrors.InvalidCredentials);
        }

        if (user.Status != UserStatus.Verified)
        {
            return Result.Failure<Token>(UserErrors.NotVerified);
        }

        if (!passwordHasher.Verify(request.Password, user.Password!))
        {
            return Result.Failure<Token>(UserErrors.InvalidCredentials);
        }

        if (!user.IsEmailVerified)
        {
            return Result.Failure<Token>(UserErrors.EmailNotVerified);
        }

        Token token = jwtTokenGenerator.GenerateToken(user);

        Result<RefreshToken> refreshTokenResult = RefreshToken.Create(token.RefreshToken);

        if (refreshTokenResult.IsFailure)
        {
            return Result.Failure<Token>(refreshTokenResult.Error);
        }

        user.SetRefreshToken(refreshTokenResult.Value, dateTimeProvider.UtcNow.AddMinutes(token.RefreshExpiresIn));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
    }
}
