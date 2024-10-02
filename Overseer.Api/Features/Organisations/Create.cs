using Carter;
using FluentValidation;
using MediatR;
using Overseer.Api.Abstractions.Encryption;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Abstractions.Time;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Organisations.Entities;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Features.Organisations;

public record CreateOrganisationRequest(string Name, string Email, string Username, string Password, string FirstName, string LastName, string Description);

public record CreateOrganisationResponse(Guid Id);

public record CreateOrganisationCommand(string Name, string Email, string Username, string Password, string FirstName, string LastName, string Description) : ICommand<Guid>;

public class CreateOrganisationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/organisations", async (
            CreateOrganisationRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateOrganisationCommand(request.Name, request.Email, request.Username, request.Password, request.FirstName, request.LastName, request.Description);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }

            return Results.Ok(new CreateOrganisationResponse(result.Value));
        })
        .WithTags(Tags.Organisations)
        .AllowAnonymous();
}

public class CreateOrganisationValidator : AbstractValidator<CreateOrganisationCommand>
{
    public CreateOrganisationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);
    }
}

public class CreateOrganisationHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<CreateOrganisationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateOrganisationCommand request, CancellationToken cancellationToken)
    {
        var organisation = Organisation.Create(request.Name, request.Description, dateTimeProvider.UtcNow);

        string hashedPassword = passwordHasher.Hash(request.Password);

        var admin = User.Create(request.Email, request.Username, hashedPassword, request.FirstName, request.LastName, organisation.Id, Role.Admin, true, null, null);

        unitOfWork.Organisations.Add(organisation);

        foreach (Role role in admin.Roles)
        {
            unitOfWork.Roles.Attach(role);
        }

        unitOfWork.Users.Add(admin);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return organisation.Id;
    }
}
