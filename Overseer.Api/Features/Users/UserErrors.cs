using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Users;

public static class UserErrors
{
    public static Error VerificationFailed => Error.Problem("Error.VerificationFailed", "Verification failed.");

    public static Error InvalidCredentials => Error.Problem("Error.InvalidCredentials", "Invalid credentials.");

    public static Error EmailNotVerified => Error.Problem("Error.EmailNotVerified", "Email not verified.");

    public static Error RefreshTokenExpired => Error.Problem("Error.RefreshTokenExpired", "Refresh token expired.");

    public static Error NotFound => Error.NotFound("Error.NotFound", "User not found.");

    public static Error NotUnique(string name) =>

        Error.Conflict("Error.NotUnique", $"Provided {name} is not unique.");
}
