using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Customers.Entities;

namespace Overseer.Api.Features.Customers.Events;

public record CustomerCreatedDomainEvent(Guid Id, DateTime OccurredOnUtc, Customer Customer) : IDomainEvent;
