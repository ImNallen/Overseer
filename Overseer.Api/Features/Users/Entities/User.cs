using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Organisations.Entities;
using Overseer.Api.Features.Users.Events;

namespace Overseer.Api.Features.Users.Entities;

public class User : Entity
{
    private readonly List<Role> _roles = new();

    // Used to create a user to an organisation.
    private User(
        Guid id,
        string email,
        string username,
        string password,
        string firstName,
        string lastName,
        Guid organisationId,
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
        OrganisationId = organisationId;
        IsEmailVerified = isEmailVerified;
        EmailVerificationToken = emailVerificationToken;
        EmailVerificationTokenExpiresAt = emailVerificationTokenExpiresAt;
    }

    // Used to create a skeleton of a user that is invited to an organisation.
    private User(
        Guid id,
        string email,
        Guid inviteToken,
        Guid organisationId)
        : base(id)
    {
        Email = email;
        InviteToken = inviteToken;
        OrganisationId = organisationId;
    }

    // The user's email address.
    public string Email { get; private set; }

    // The user's username.
    public string? Username { get; private set; }

    // The user's password. This is hashed before being stored.
    public string? Password { get; private set; }

    // The user's first name.
    public string? FirstName { get; private set; }

    // The user's last name.
    public string? LastName { get; private set; }

    // The invite token for the user. This is used to register a user that has been invited to an organisation.
    public Guid? InviteToken { get; private set; }

    // Whether the user's email address has been verified.
    public bool IsEmailVerified { get; private set; }

    // The email verification token for the user. This is used to verify the user's email address.
    public Guid? EmailVerificationToken { get; private set; }

    // The expiry date for the email verification token.
    public DateTime? EmailVerificationTokenExpiresAt { get; private set; }

    // The password reset token for the user. This is used to reset the user's password.
    public Guid? PasswordResetToken { get; private set; }

    // The expiry date for the password reset token.
    public DateTime? PasswordResetTokenExpires { get; private set; }

    // The refresh token for the user. This is used to refresh the user's access token.
    public string? RefreshToken { get; private set; }

    // The expiry date for the refresh token.
    public DateTime? RefreshTokenExpires { get; private set; }

    // The status of the user. Can be one of the following:
    // - Invited: The user has been invited to an organisation but has not registered yet.
    // - Registered: The user has registered but not verified their email address.
    // - Verified: The user has verified their email address.
    public UserStatus Status { get; private set; }

    // The ID of the organisation that the user belongs to.
    public Guid OrganisationId { get; private set; }

    // Navigation property for the organisation.
    public virtual Organisation Organisation { get; private set; } = null!;

    // Navigation property for the roles.
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    // Used to create a skeleton of a user that is invited to an organisation. The full user is later created by the Register method.
    public static User CreateInvitedUser(
        string email,
        Guid inviteToken,
        Guid organisationId,
        Role role)
    {
        var user = new User(
            Guid.NewGuid(),
            email,
            inviteToken,
            organisationId);

        user._roles.Add(role);

        user.Status = UserStatus.Invited;

        return user;
    }

    // Used to create a user that is not invited to an organisation. This creates a full user.
    public static User Create(
        string email,
        string username,
        string password,
        string firstName,
        string lastName,
        Guid organisationId,
        Role role,
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
            organisationId,
            isEmailVerified,
            emailVerificationToken,
            emailVerificationTokenExpiresAt);

        user._roles.Add(role);

        user.Status = isEmailVerified ? UserStatus.Verified : UserStatus.Registered;

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(Guid.NewGuid(), DateTime.UtcNow, user.Email, user.EmailVerificationToken));

        return user;
    }

    // Used to finish the registration of a user that has been invited to an organisation.
    public User Register(
        string username,
        string password,
        string firstName,
        string lastName,
        bool isEmailVerified = false,
        Guid? emailVerificationToken = null,
        DateTime? emailVerificationTokenExpiresAt = null)
    {
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;

        Status = isEmailVerified ? UserStatus.Verified : UserStatus.Registered;

        IsEmailVerified = isEmailVerified;
        EmailVerificationToken = emailVerificationToken;
        EmailVerificationTokenExpiresAt = emailVerificationTokenExpiresAt;

        InviteToken = null;

        RaiseDomainEvent(new UserRegisteredDomainEvent(Guid.NewGuid(), DateTime.UtcNow, Email, emailVerificationToken));

        return this;
    }

    // Used to set a refresh token on a user and set the expiry date.
    public User SetRefreshToken(string refreshToken, DateTime expires)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpires = expires;

        return this;
    }

    // Used to verify a user's email.
    public User VerifyEmail()
    {
        IsEmailVerified = true;
        EmailVerificationToken = null;
        EmailVerificationTokenExpiresAt = null;

        return this;
    }

    // Used to set a password reset token on a user and set the expiry date.
    public User SetPasswordResetToken(Guid passwordResetToken, DateTime expires)
    {
        PasswordResetToken = passwordResetToken;
        PasswordResetTokenExpires = expires;

        return this;
    }

    // Used to reset a user's password.
    public User ResetPassword(string password)
    {
        Password = password;
        PasswordResetToken = null;
        PasswordResetTokenExpires = null;

        return this;
    }

    // Used to update a user's first name and last name.
    public User Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        return this;
    }
}
