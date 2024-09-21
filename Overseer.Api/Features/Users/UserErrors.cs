using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Users;

public static class UserErrors
{
    public static Error NotFound => Error.NotFound("Error.NotFound", "User not found.");

    public static Error NotUnique(string name) =>
        Error.Conflict("Error.NotUnique", $"Provided {name} is not unique.");
}
