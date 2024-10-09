using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Customers.Entities;

public static class CustomerErrors
{
    public static Error EmailInUse => Error.Conflict("Customer.EmailInUse", "The email is already in use.");
}
