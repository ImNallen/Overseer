using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Abstractions.Encryption;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Features.Users;

public record ResetRequest(string Password);

public record ResetCommand(Guid Token, string Password) : ICommand;

public class ResetEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/users/reset/{token:guid}", async (
            Guid token,
            ResetRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new ResetCommand(token, request.Password);

            Result result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }

            return Results.Ok();
        })
        .WithTags(Tags.Users)
        .AllowAnonymous();
}

public class ResetHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : ICommandHandler<ResetCommand>
{
    public async Task<Result> Handle(ResetCommand request, CancellationToken cancellationToken)
    {
        User? user = await unitOfWork.Users.FirstOrDefaultAsync(x => x.PasswordResetToken == request.Token, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        string hashedPassword = passwordHasher.Hash(request.Password);

        Result<Password> passwordResult = Password.Create(hashedPassword);

        if (passwordResult.IsFailure)
        {
            return Result.Failure(passwordResult.Error);
        }

        user.ResetPassword(passwordResult.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
