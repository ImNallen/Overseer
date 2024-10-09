using System.Globalization;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Features.Shared;

public record FirstName
{
    public static readonly int MinLength = 2;

    public static readonly int MaxLength = 100;

    private FirstName(string value) => Value = value;

    public string Value { get; }

    public static explicit operator string(FirstName firstName) => firstName.Value;

    public static Result<FirstName> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Failure<FirstName>(FirstNameErrors.Empty);
        }

        value = value.ToLower(CultureInfo.CurrentCulture);

        if (value.Length < MinLength)
        {
            return Result.Failure<FirstName>(FirstNameErrors.TooShort);
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<FirstName>(FirstNameErrors.TooLong);
        }

        return new FirstName(value);
    }
}
