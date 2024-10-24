using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Persistence;
using Overseer.Api.Utilities.Exceptions;

namespace Overseer.Api.Features.Users;

public record DeleteUserCommand(Guid Id) : ICommand;

public class DeleteUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapDelete("/users/{id}", async (
            Guid id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteUserCommand(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : CustomResults.Problem(result);
        })
        .WithTags(Tags.Users)
        .RequireAuthorization(Permissions.Admin);
}

public class DeleteUserHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await unitOfWork.Users
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        unitOfWork.Users.Remove(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
