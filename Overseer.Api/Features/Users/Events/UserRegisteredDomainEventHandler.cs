using FluentEmail.Core;
using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Users.Events;

public class UserRegisteredDomainEventHandler(IFluentEmail fluentEmail) : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken) =>
        await fluentEmail.To(notification.User.Email)
            .Subject("Welcome to Overseer")
            .Body("Welcome to Overseer", isHtml: true)
            .SendAsync(cancellationToken);
}
