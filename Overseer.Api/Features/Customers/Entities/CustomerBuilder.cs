using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Shared;

namespace Overseer.Api.Features.Customers.Entities;

public class CustomerBuilder
{
    public Email? Email { get; private set; }

    public FirstName? FirstName { get; private set; }

    public LastName? LastName { get; private set; }

    public Address? Address { get; private set; }

    public static CustomerBuilder Empty() => new();

    public CustomerBuilder WithEmail(Email email)
    {
        Email = email;

        return this;
    }

    public CustomerBuilder WithName(FirstName firstName, LastName lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        return this;
    }

    public CustomerBuilder WithAddress(Address address)
    {
        Address = address;

        return this;
    }

    public Customer Build(DateTime createdAt)
    {
        Ensure.NotNull(Email);
        Ensure.NotNull(FirstName);
        Ensure.NotNull(LastName);
        Ensure.NotNull(Address);

        return Customer.Create(Email, FirstName, LastName, Address, createdAt);
    }
}
