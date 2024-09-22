using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Abstractions.Encryption;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Abstractions.Time;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Services.Authentication;

namespace Overseer.Api.Features.Users;

public record LoginRequest(string Email, string Password);

public record TokenResponse(string AccessToken, int ExpiresIn, string RefreshToken, int RefreshExpiresIn);

public record LoginCommand(string Email, string Password) : ICommand<Token>;

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/users/login", async (
            LoginRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginCommand(request.Email, request.Password);

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
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

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
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<Token>(UserErrors.InvalidCredentials);
        }

        if (!passwordHasher.Verify(request.Password, user.Password))
        {
            return Result.Failure<Token>(UserErrors.InvalidCredentials);
        }

        if (!user.IsEmailVerified)
        {
            return Result.Failure<Token>(UserErrors.EmailNotVerified);
        }

        Token token = jwtTokenGenerator.GenerateToken(user);

        user.SetRefreshToken(token.RefreshToken, dateTimeProvider.UtcNow.AddMinutes(token.RefreshExpiresIn));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
    }
}
