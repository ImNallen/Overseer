using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Users.Entities;

public static class UsernameErrors
{
    public static Error Empty => Error.Validation("Username.Empty", "The username is empty.");

    public static Error TooShort => Error.Validation("Username.TooShort", "The username is too short.");

    public static Error TooLong => Error.Validation("Username.TooLong", "The username is too long.");
}
