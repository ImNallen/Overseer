using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Shared;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Persistence;
using Overseer.Api.Services.Encryption;
using Overseer.Api.Services.Time;
using Overseer.Api.Utilities.Exceptions;

namespace Overseer.Api.Features.Users;

public record CreateUserRequest(
    string Email,
    string Username,
    string Password,
    string FirstName,
    string LastName,
    string Role);

public record CreateUserResponse(Guid Id);

public record CreateUserCommand(
    string Email,
    string Username,
    string Password,
    string FirstName,
    string LastName,
    string Role,
    bool? Verified = false) : ICommand<Guid>;

public class CreateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/users", async (
                CreateUserRequest request,
                [FromQuery] bool? verified,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateUserCommand(
                    request.Email,
                    request.Username,
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

                return Results.Ok(new CreateUserResponse(result.Value));
            })
            .WithTags(Tags.Users)
            .AllowAnonymous();
}

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(Email.MaxLength);

        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(Username.MinLength)
            .MaximumLength(Username.MaxLength);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(Password.MinLength)
            .MaximumLength(Password.MaxLength);

        RuleFor(x => x.FirstName)
            .MinimumLength(FirstName.MinLength)
            .MaximumLength(FirstName.MaxLength)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .MinimumLength(LastName.MinLength)
            .MaximumLength(LastName.MaxLength)
            .NotEmpty();
    }
}

public class CreateUserHandler(
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IPasswordHasher passwordHasher)
    : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(request.Email);
        Result<Username> usernameResult = Username.Create(request.Username);
        string hashedPassword = passwordHasher.Hash(request.Password);
        Result<Password> passwordResult = Password.Create(hashedPassword);
        Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
        Result<LastName> lastNameResult = LastName.Create(request.LastName);

        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        if (usernameResult.IsFailure)
        {
            return Result.Failure<Guid>(usernameResult.Error);
        }

        if (passwordResult.IsFailure)
        {
            return Result.Failure<Guid>(passwordResult.Error);
        }

        if (firstNameResult.IsFailure)
        {
            return Result.Failure<Guid>(firstNameResult.Error);
        }

        if (lastNameResult.IsFailure)
        {
            return Result.Failure<Guid>(lastNameResult.Error);
        }

        bool isEmailInUse = await unitOfWork.Users.AnyAsync(
            user => (string)user.Email == request.Email,
            cancellationToken);

        bool isUsernameInUse = await unitOfWork.Users.AnyAsync(
            user => (string)user.Username == request.Username,
            cancellationToken);

        if (isEmailInUse)
        {
            return Result.Failure<Guid>(UserErrors.NotUnique(nameof(request.Email)));
        }

        if (isUsernameInUse)
        {
            return Result.Failure<Guid>(UserErrors.NotUnique(nameof(request.Username)));
        }

        Role roleToAssign = Role.All.Select(role => role.Name).Contains(request.Role)
            ? Role.All.First(role => role.Name == request.Role)
            : Role.User;

        User user = UserBuilder
            .Empty()
            .WithEmail(emailResult.Value)
            .WithUsername(usernameResult.Value)
            .WithPassword(passwordResult.Value)
            .WithName(firstNameResult.Value, lastNameResult.Value)
            .WithRole(roleToAssign)
            .Verified(request.Verified ?? false)
            .RequireEmailVerification(
                request.Verified ?? false ? null : Guid.NewGuid(),
                request.Verified ?? false ? DateTime.MinValue : dateTimeProvider.UtcNow.AddDays(5))
            .Build(dateTimeProvider.UtcNow);

        foreach (Role role in user.Roles)
        {
            unitOfWork.Roles.Attach(role);
        }

        await unitOfWork.Users.AddAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
