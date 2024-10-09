namespace Overseer.Api.Features.Users.Entities;

public enum UserStatus
{
    /// <summary>
    /// The user has registered in the system.
    /// </summary>
    Registered,

    /// <summary>
    /// The user has verified their email address.
    /// </summary>
    Verified
}
