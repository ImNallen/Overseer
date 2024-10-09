using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Shared;

public static class EmailErrors
{
    public static Error Invalid => Error.Validation("Email.Invalid", "The email is invalid.");

    public static Error TooLong => Error.Validation("Email.TooLong", "The email is too long.");

    public static Error Empty => Error.Validation("Email.Empty", "The email is empty.");
}
