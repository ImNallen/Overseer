using FluentEmail.Core;
using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Customers.Events;

public class CustomerCreatedDomainEventHandler(IFluentEmail fluentEmail) : IDomainEventHandler<CustomerCreatedDomainEvent>
{
    public async Task Handle(CustomerCreatedDomainEvent notification, CancellationToken cancellationToken) => await fluentEmail
            .To(notification.Customer.Email.Value)
            .Subject("Welcome to Overseer")
            .Body($"Welcome to Overseer, {notification.Customer.FirstName.Value} {notification.Customer.LastName.Value}!")
            .SendAsync(cancellationToken);
}
