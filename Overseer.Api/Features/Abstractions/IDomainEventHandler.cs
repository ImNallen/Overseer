using MediatR;

namespace Overseer.Api.Features.Abstractions;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;
