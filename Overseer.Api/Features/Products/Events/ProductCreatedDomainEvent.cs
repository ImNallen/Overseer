using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Products.Events;

public record ProductCreatedDomainEvent(Guid Id, DateTime OccurredOnUtc, Guid ProductId, string Name, string Description, decimal Price) : IDomainEvent;
