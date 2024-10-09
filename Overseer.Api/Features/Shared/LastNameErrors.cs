using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Shared;

public static class LastNameErrors
{
    public static Error TooShort => Error.Validation("LastName.TooShort", "The last name is too short.");

    public static Error TooLong => Error.Validation("LastName.TooLong", "The last name is too long.");

    public static Error Empty => Error.Validation("LastName.Empty", "The last name is empty.");
}
