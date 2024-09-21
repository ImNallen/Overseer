using MediatR;

namespace Overseer.Api.Features.Abstractions;

public interface IDomainEvent : INotification
{
    Guid Id { get; init; }

    DateTime OccurredOnUtc { get; init; }
}
