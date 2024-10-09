using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Users.Entities;

public static class PasswordErrors
{
    public static Error Empty => Error.Validation("Password.Empty", "The password is empty.");

    public static Error TooShort => Error.Validation("Password.TooShort", "The password is too short.");

    public static Error TooLong => Error.Validation("Password.TooLong", "The password is too long.");
}
