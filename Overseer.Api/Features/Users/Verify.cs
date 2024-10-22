using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Persistence;
using Overseer.Api.Utilities.Exceptions;

namespace Overseer.Api.Features.Users;

public record VerifyCommand(Guid Token) : ICommand;

public class VerifyEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/users/verify", async (
            Guid token,
            ISender sender,
            CancellationToken cancellationToken) =>
            {
                var command = new VerifyCommand(token);

                Result result = await sender.Send(command, cancellationToken);

                if (result.IsFailure)
                {
                    return CustomResults.Problem(result);
                }

                return Results.NoContent();
            })
            .WithTags(Tags.Users)
            .AllowAnonymous();
}

public class VerifyValidator : AbstractValidator<VerifyCommand>
{
    public VerifyValidator() => RuleFor(x => x.Token)
        .NotEmpty();
}

public class VerifyHandler(
    IUnitOfWork unitOfWork)
    : ICommandHandler<VerifyCommand>
{
    public async Task<Result> Handle(VerifyCommand request, CancellationToken cancellationToken)
    {
        User? user = await unitOfWork.Users
            .Where(x => x.EmailVerificationToken == request.Token)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.VerificationFailed);
        }

        user.VerifyEmail();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
