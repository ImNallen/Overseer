using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Shared;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Persistence;
using Overseer.Api.Services.Authentication;
using Overseer.Api.Utilities.Exceptions;

namespace Overseer.Api.Features.Users;

public record UpdateUserRequest(string FirstName, string LastName);

public record UpdateUserResponse(Guid Id, string Email, string Username, string FirstName, string LastName);

public record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : ICommand<User>;

public class UpdateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPut("/users/profile", async (
            UpdateUserRequest request,
            ClaimsPrincipal claims,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateUserCommand(claims.GetUserId(), request.FirstName, request.LastName);

            Result<User> result = await sender.Send(command, cancellationToken);

            return result.IsFailure ? CustomResults.Problem(result) : Results.Ok(new UpdateUserResponse(result.Value.Id, result.Value.Email.Value, result.Value.Username.Value, result.Value.FirstName.Value, result.Value.LastName.Value));
        })
        .WithTags(Tags.Users)
        .RequireAuthorization(Permissions.UsersWrite);
}

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(FirstName.MinLength)
            .MaximumLength(FirstName.MaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(LastName.MinLength)
            .MaximumLength(LastName.MaxLength);
    }
}

public class UpdateUserHandler(
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand, User>
{
    public async Task<Result<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await unitOfWork.Users
            .Where(x => x.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<User>(UserErrors.NotFound);
        }

        if (user.Status != UserStatus.Verified)
        {
            return Result.Failure<User>(UserErrors.NotVerified);
        }

        Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
        Result<LastName> lastNameResult = LastName.Create(request.LastName);

        if (firstNameResult.IsFailure)
        {
            return Result.Failure<User>(firstNameResult.Error);
        }

        if (lastNameResult.IsFailure)
        {
            return Result.Failure<User>(lastNameResult.Error);
        }

        user.Update(firstNameResult.Value, lastNameResult.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }
}
