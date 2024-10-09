using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Users.Entities;

public static class UserErrors
{
    public static Error VerificationFailed => Error.Problem("Users.VerificationFailed", "Verification failed.");

    public static Error InvalidCredentials => Error.Problem("Users.InvalidCredentials", "Invalid credentials.");

    public static Error EmailNotVerified => Error.Problem("Users.EmailNotVerified", "Email not verified.");

    public static Error RefreshTokenExpired => Error.Problem("Users.RefreshTokenExpired", "Refresh token expired.");

    public static Error NotFound => Error.NotFound("Users.NotFound", "User not found.");

    public static Error NotVerified => Error.Problem("Users.NotVerified", "User not verified.");

    public static Error NotUnique(string name) =>
        Error.Conflict("Users.NotUnique", $"Provided {name} is not unique.");

    public static Error Empty(string name) =>
        Error.Problem("Users.Empty", $"Provided {name} is empty.");

    public static Error TooShort(string name, int length) =>
        Error.Problem("Users.TooShort", $"Provided {name} is too short. It must be at least {length} characters long.");

    public static Error TooLong(string name, int length) =>
        Error.Problem("Users.TooLong", $"Provided {name} is too long. It must be at most {length} characters long.");

    public static Error InvalidFormat(string name) =>
        Error.Problem("Users.InvalidFormat", $"Provided {name} is invalid.");
}
