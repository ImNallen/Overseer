using Carter;
using FluentValidation;
using MediatR;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Messaging;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Products.Entities;

namespace Overseer.Api.Features.Products;

public record CreateProductRequest(string Name, string Description, decimal Price);

public record CreateProductResponse(Guid Id);

public record CreateProductCommand(string Name, string Description, decimal Price) : ICommand<Guid>;

public class CreateProduct : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app) =>
        app.MapPost("/products", async (
            CreateProductRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
            {
                CreateProductCommand command = new(
                    request.Name,
                    request.Description,
                    request.Price);

                Result<Guid> result = await sender.Send(command, cancellationToken);

                if (result.IsFailure)
                {
                    return CustomResults.Problem(result);
                }

                return Results.Created($"products/{result.Value}", new CreateProductResponse(result.Value));
            })
            .WithTags(Tags.Products)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(Permissions.ProductsWrite);
}

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}

public class CreateProductHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = Product.Create(request.Name, request.Description, request.Price);

        await unitOfWork.Products.AddAsync(product, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
