using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Customers.Events;
using Overseer.Api.Features.Shared;

namespace Overseer.Api.Features.Customers.Entities;

public class Customer : Entity
{
    private Customer(
        Guid id,
        Email email,
        FirstName firstName,
        LastName lastName,
        Address address,
        DateTime createdAt)
        : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        CreatedAt = createdAt;
    }

    #pragma warning disable CS8618
    private Customer()
    {
    }
    #pragma warning restore CS8618

    public Email Email { get; private set; }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Address Address { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static Customer Create(
        Email email,
        FirstName firstName,
        LastName lastName,
        Address address,
        DateTime createdAt)
    {
        var customer = new Customer(
            Guid.NewGuid(),
            email,
            firstName,
            lastName,
            address,
            createdAt);

        customer.RaiseDomainEvent(new CustomerCreatedDomainEvent(Guid.NewGuid(), DateTime.UtcNow, customer));

        return customer;
    }
}
