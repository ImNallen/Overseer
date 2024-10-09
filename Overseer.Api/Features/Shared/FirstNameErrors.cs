using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Shared;

public static class FirstNameErrors
{
    public static Error TooShort => Error.Validation("FirstName.TooShort", "The first name is too short.");

    public static Error TooLong => Error.Validation("FirstName.TooLong", "The first name is too long.");

    public static Error Empty => Error.Validation("FirstName.Empty", "The first name is empty.");
}
