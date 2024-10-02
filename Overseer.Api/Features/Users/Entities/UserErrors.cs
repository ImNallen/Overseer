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
}
