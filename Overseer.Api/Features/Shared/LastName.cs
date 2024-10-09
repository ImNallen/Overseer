using System.Globalization;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Features.Shared;

public record LastName
{
    public static readonly int MinLength = 2;

    public static readonly int MaxLength = 100;

    private LastName(string value) => Value = value;

    public string Value { get; }

    public static explicit operator string(LastName lastName) => lastName.Value;

    public static Result<LastName> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Failure<LastName>(LastNameErrors.Empty);
        }

        value = value.ToLower(CultureInfo.CurrentCulture);

        if (value.Length < MinLength)
        {
            return Result.Failure<LastName>(LastNameErrors.TooShort);
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<LastName>(LastNameErrors.TooLong);
        }

        return new LastName(value);
    }
}
