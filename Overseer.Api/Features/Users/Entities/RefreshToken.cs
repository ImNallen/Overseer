using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Users.Entities;

public record RefreshToken
{
    private RefreshToken(string value) => Value = value;

    public string Value { get; }

    public static explicit operator string(RefreshToken refreshToken) => refreshToken.Value;

    public static Result<RefreshToken> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Failure<RefreshToken>(UserErrors.Empty(nameof(RefreshToken)));
        }

        return new RefreshToken(value);
    }
}
