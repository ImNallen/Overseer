using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Users.Entities;

public sealed record Username
{
    public static readonly int MinLength = 3;

    public static readonly int MaxLength = 50;

    private Username(string value) => Value = value;

    public string Value { get; }

    public static explicit operator string(Username username) => username.Value;

    public static Result<Username> Create(string? username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return Result.Failure<Username>(UsernameErrors.Empty);
        }

        if (username.Length < MinLength)
        {
            return Result.Failure<Username>(UsernameErrors.TooShort);
        }

        if (username.Length > MaxLength)
        {
            return Result.Failure<Username>(UsernameErrors.TooLong);
        }

        return new Username(username);
    }
}
