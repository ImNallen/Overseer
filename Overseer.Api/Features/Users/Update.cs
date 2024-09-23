using System.Security.Claims;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Services.Authentication;

namespace Overseer.Api.Features.Users;

public record UpdateUserRequest(string FirstName, string LastName);

public record UpdateUserResponse(Guid Id, string Email, string FirstName, string LastName);

public record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : ICommand<User>;

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPut("/users/profile", async (
            UpdateUserRequest request,
            ClaimsPrincipal claims,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateUserCommand(claims.GetUserId(), request.FirstName, request.LastName);

            Result<User> result = await sender.Send(command, cancellationToken);

            return result.IsFailure ? CustomResults.Problem(result) : Results.Ok(new UpdateUserResponse(result.Value.Id, result.Value.Email, result.Value.FirstName, result.Value.LastName));
        })
        .WithTags(Tags.Users)
        .RequireAuthorization(Permissions.UsersWrite);
}

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
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

        user.Update(request.FirstName, request.LastName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }
}
