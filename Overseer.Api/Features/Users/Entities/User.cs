using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Shared;
using Overseer.Api.Features.Users.Events;

namespace Overseer.Api.Features.Users.Entities;

public class User : Entity
{
    private readonly List<Role> _roles = new();

    private User(
        Guid id,
        Email email,
        Username username,
        Password password,
        FirstName firstName,
        LastName lastName,
        DateTime createdAt,
        bool isEmailVerified = false,
        Guid? emailVerificationToken = null,
        DateTime? emailVerificationTokenExpiresAt = null)
        : base(id)
    {
        Email = email;
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        CreatedAt = createdAt;
        IsEmailVerified = isEmailVerified;
        EmailVerificationToken = emailVerificationToken;
        EmailVerificationTokenExpiresAt = emailVerificationTokenExpiresAt;
    }

    public Email Email { get; private set; }

    public Username Username { get; private set; }

    public Password Password { get; private set; }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public bool IsEmailVerified { get; private set; }

    public Guid? EmailVerificationToken { get; private set; }

    public DateTime? EmailVerificationTokenExpiresAt { get; private set; }

    public Guid? PasswordResetToken { get; private set; }

    public DateTime? PasswordResetTokenExpires { get; private set; }

    public RefreshToken? RefreshToken { get; private set; }

    public DateTime? RefreshTokenExpires { get; private set; }

    public UserStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public static User Create(
        Email email,
        Username username,
        Password password,
        FirstName firstName,
        LastName lastName,
        Role role,
        DateTime createdAt,
        bool isEmailVerified = false,
        Guid? emailVerificationToken = null,
        DateTime? emailVerificationTokenExpiresAt = null)
    {
        var user = new User(
            Guid.NewGuid(),
            email,
            username,
            password,
            firstName,
            lastName,
            createdAt,
            isEmailVerified,
            emailVerificationToken,
            emailVerificationTokenExpiresAt);

        user._roles.Add(role);

        user.Status = isEmailVerified ? UserStatus.Verified : UserStatus.Registered;

        user.RaiseDomainEvent(
            new UserRegisteredDomainEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                user.Email.Value,
                user.EmailVerificationToken));

        return user;
    }

    public User SetRefreshToken(
        RefreshToken refreshToken,
        DateTime expires)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpires = expires;

        return this;
    }

    public User VerifyEmail()
    {
        IsEmailVerified = true;
        Status = UserStatus.Verified;

        EmailVerificationToken = null;
        EmailVerificationTokenExpiresAt = null;

        return this;
    }

    public User SetPasswordResetToken(
        Guid passwordResetToken,
        DateTime expires)
    {
        PasswordResetToken = passwordResetToken;
        PasswordResetTokenExpires = expires;

        return this;
    }

    public User ResetPassword(Password password)
    {
        Password = password;
        PasswordResetToken = null;
        PasswordResetTokenExpires = null;

        return this;
    }

    public User Update(
        FirstName firstName,
        LastName lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        return this;
    }
}
