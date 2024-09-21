using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Events;

namespace Overseer.Api.Features.Users;

public class User : Entity
{
    private User(
        Guid id,
        string email,
        string password,
        string firstName,
        string lastName,
        bool isEmailVerified = false,
        Guid? emailVerificationToken = null,
        DateTime? emailVerificationTokenExpiresAt = null)
        : base(id)
    {
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        IsEmailVerified = isEmailVerified;
        EmailVerificationToken = emailVerificationToken;
        EmailVerificationTokenExpiresAt = emailVerificationTokenExpiresAt;
    }

    public string Email { get; private set; }

    public string Password { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public bool IsEmailVerified { get; private set; }

    public Guid? EmailVerificationToken { get; private set; }

    public DateTime? EmailVerificationTokenExpiresAt { get; private set; }

    public Guid? PasswordResetToken { get; private set; }

    public DateTime? PasswordResetTokenExpires { get; private set; }

    public string? RefreshToken { get; private set; }

    public static User Create(
        string email,
        string password,
        string firstName,
        string lastName,
        bool isEmailVerified = false,
        Guid? emailVerificationToken = null,
        DateTime? emailVerificationTokenExpiresAt = null)
    {
        var user = new User(
            Guid.NewGuid(),
            email,
            password,
            firstName,
            lastName,
            isEmailVerified,
            emailVerificationToken,
            emailVerificationTokenExpiresAt);

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(Guid.NewGuid(), DateTime.UtcNow, user));

        return user;
    }

    public User SetRefreshToken(string refreshToken)
    {
        RefreshToken = refreshToken;

        return this;
    }

    public User SetPasswordResetToken(Guid passwordResetToken, DateTime expires)
    {
        PasswordResetToken = passwordResetToken;
        PasswordResetTokenExpires = expires;

        return this;
    }

    public User VerifyEmail()
    {
        IsEmailVerified = true;
        EmailVerificationToken = null;
        EmailVerificationTokenExpiresAt = null;

        return this;
    }

    public User ResetPassword(string password)
    {
        Password = password;
        PasswordResetToken = null;
        PasswordResetTokenExpires = null;

        return this;
    }
}
