using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Customers.Entities;
using Overseer.Api.Features.Shared;
using Overseer.Api.Persistence;
using Overseer.Api.Services.Time;
using Overseer.Api.Utilities.Exceptions;

namespace Overseer.Api.Features.Customers;

public record CreateCustomerRequest(string Email, string FirstName, string LastName, string Address, string City, string State, string ZipCode);

public record CreateCustomerResponse(Guid Id);

public record CreateCustomerCommand(string Email, string FirstName, string LastName, string Address, string City, string State, string ZipCode) : ICommand<Guid>;

public class CreateCustomerEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/customers", async (
            CreateCustomerRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateCustomerCommand(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Address,
                request.City,
                request.State,
                request.ZipCode);

            Result<Guid> result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return CustomResults.Problem(result);
            }

            return Results.Ok(new CreateCustomerResponse(result.Value));
        })
        .WithTags(Tags.Customers)
        .RequireAuthorization(Permissions.CustomersWrite);
}

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(Email.MaxLength);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(FirstName.MinLength)
            .MaximumLength(FirstName.MaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(LastName.MinLength)
            .MaximumLength(LastName.MaxLength);

        RuleFor(x => x.Address).NotEmpty();
    }
}

public class CreateCustomerHandler(
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateCustomerCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(request.Email);
        Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
        Result<LastName> lastNameResult = LastName.Create(request.LastName);

        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        if (firstNameResult.IsFailure)
        {
            return Result.Failure<Guid>(firstNameResult.Error);
        }

        if (lastNameResult.IsFailure)
        {
            return Result.Failure<Guid>(lastNameResult.Error);
        }

        bool isEmailInUse = await unitOfWork.Customers.AnyAsync(
            customer => (string)customer.Email == request.Email,
            cancellationToken);

        if (isEmailInUse)
        {
            return Result.Failure<Guid>(CustomerErrors.EmailInUse);
        }

        var address = new Address(request.Address, request.City, request.State, request.ZipCode);

        Customer customer = CustomerBuilder
            .Empty()
            .WithEmail(emailResult.Value)
            .WithName(firstNameResult.Value, lastNameResult.Value)
            .WithAddress(address)
            .Build(dateTimeProvider.UtcNow);

        await unitOfWork.Customers.AddAsync(customer, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
