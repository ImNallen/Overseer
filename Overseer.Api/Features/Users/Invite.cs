using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Organisations.Entities;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Features.Users.Invite;

public record InviteUserRequest(string Email, string Role);

public record InviteUserCommand(string Email, string Role) : ICommand<Guid>;

public class InviteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/users/invite", async (
            InviteUserRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new InviteUserCommand(request.Email, request.Role);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }

            return Results.Ok(result.Value);
        })
        .WithTags(Tags.Users)
        .RequireAuthorization(Permissions.Admin);
}

public class InviteUserValidator : AbstractValidator<InviteUserCommand>
{
    public InviteUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(x => Role.All.Exists(r => r.Name == x))
            .WithMessage("Invalid role");
    }
}

public class InviteUserHandler(IUnitOfWork unitOfWork) : ICommandHandler<InviteUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(InviteUserCommand request, CancellationToken cancellationToken)
    {
        Organisation? organisation = await unitOfWork.Organisations.FirstOrDefaultAsync(cancellationToken);

        if (organisation is null)
        {
            return Result.Failure<Guid>(OrganisationErrors.NotFound);
        }

        bool isEmailInUse = await unitOfWork.Users.AnyAsync(
            user => user.Email == request.Email,
            cancellationToken);

        if (isEmailInUse)
        {
            return Result.Failure<Guid>(UserErrors.NotUnique(nameof(User.Email)));
        }

        Role roleToAssign = Role.All.First(r => r.Name == request.Role);

        var user = User.CreateInvitedUser(request.Email, Guid.NewGuid(), organisation.Id, roleToAssign);

        foreach (Role role in user.Roles)
        {
            unitOfWork.Roles.Attach(role);
        }

        await unitOfWork.Users.AddAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
