using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Shared;

namespace Overseer.Api.Features.Users.Entities;

public class UserBuilder
{
    public Email? Email { get; private set; }

    public Username? Username { get; private set; }

    public Password? Password { get; private set; }

    public FirstName? FirstName { get; private set; }

    public LastName? LastName { get; private set; }

    public Role? Role { get; private set; }

    public bool IsEmailVerified { get; private set; }

    public Guid? EmailVerificationToken { get; private set; }

    public DateTime? EmailVerificationTokenExpiresAt { get; private set; }

    public static UserBuilder Empty() => new();

    public UserBuilder WithEmail(Email email)
    {
        Email = email;

        return this;
    }

    public UserBuilder WithUsername(Username username)
    {
        Username = username;

        return this;
    }

    public UserBuilder WithPassword(Password password)
    {
        Password = password;

        return this;
    }

    public UserBuilder WithRole(Role role)
    {
        Role = role;

        return this;
    }

    public UserBuilder WithName(FirstName firstName, LastName lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        return this;
    }

    public UserBuilder Verified(bool isVerified)
    {
        IsEmailVerified = isVerified;

        return this;
    }

    public UserBuilder RequireEmailVerification(Guid? token, DateTime? expiresAt)
    {
        EmailVerificationToken = token;
        EmailVerificationTokenExpiresAt = expiresAt;

        return this;
    }

    public User Build(DateTime createdAt)
    {
        Ensure.NotNull(Email);
        Ensure.NotNull(Username);
        Ensure.NotNull(Password);
        Ensure.NotNull(FirstName);
        Ensure.NotNull(LastName);
        Ensure.NotNull(Role);

        if (!IsEmailVerified)
        {
            Ensure.NotNull(EmailVerificationToken);
            Ensure.NotNull(EmailVerificationTokenExpiresAt);
        }

        return User.Create(
            Email,
            Username,
            Password,
            FirstName,
            LastName,
            Role,
            createdAt,
            IsEmailVerified,
            EmailVerificationToken ?? null,
            EmailVerificationTokenExpiresAt ?? null);
    }
}
