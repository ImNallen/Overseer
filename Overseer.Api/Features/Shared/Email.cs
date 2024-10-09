using System.Globalization;
using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Shared;

public sealed record Email
{
    public static readonly int MaxLength = 255;

    private Email(string value) => Value = value;

    public string Value { get; }

    public static explicit operator string(Email email) => email.Value;

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<Email>(EmailErrors.Empty);
        }

        email = email.ToLower(CultureInfo.CurrentCulture);

        if (email.Split('@').Length != 2)
        {
            return Result.Failure<Email>(EmailErrors.Invalid);
        }

        if (email.Length > MaxLength)
        {
            return Result.Failure<Email>(EmailErrors.TooLong);
        }

        return new Email(email);
    }
}
