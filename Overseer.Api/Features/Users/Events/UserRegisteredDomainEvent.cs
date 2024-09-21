using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Users.Events;

public record UserRegisteredDomainEvent(Guid Id, DateTime OccurredOnUtc, User User) : IDomainEvent;
