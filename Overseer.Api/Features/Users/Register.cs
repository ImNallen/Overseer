using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Abstractions.Encryption;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Abstractions.Time;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Features.Users;

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role);

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role,
    bool? Verified = false) : ICommand<Guid>;

public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/users/register", async (
                RegisterRequest request,
                [FromQuery] bool? verified,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new RegisterCommand(
                    request.Email,
                    request.Password,
                    request.FirstName,
                    request.LastName,
                    request.Role,
                    verified);

                Result<Guid> result = await sender.Send(command, cancellationToken);

                if (result.IsFailure)
                {
                    return CustomResults.Problem(result);
                }

                return Results.Ok(result.Value);
            })
            .WithTags(Tags.Users)
            .AllowAnonymous();
}

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(x => Role.All.Exists(r => r.Name == x))
            .WithMessage("Invalid role");
    }
}

public class RegisterHandler(
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPasswordHasher passwordHasher)
    : ICommandHandler<RegisterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
        {
            bool isEmailInUse = await unitOfWork.Users.AnyAsync(
                user => user.Email == request.Email,
                cancellationToken);

            if (isEmailInUse)
            {
                return Result.Failure<Guid>(UserErrors.NotUnique(nameof(request.Email)));
            }

            Role roleToAssign = Role.All.First(r => r.Name == request.Role);

            var user = User.Create(
                request.Email,
                passwordHasher.Hash(request.Password),
                request.FirstName,
                request.LastName,
                roleToAssign,
                isEmailVerified: request.Verified ?? false,
                emailVerificationToken: request.Verified ?? false ? null : Guid.NewGuid(),
                emailVerificationTokenExpiresAt: request.Verified ?? false ? null : dateTimeProvider.UtcNow.AddDays(5));

            foreach (Role role in user.Roles)
            {
                unitOfWork.Roles.Attach(role);
            }

            await unitOfWork.Users.AddAsync(user, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
}
