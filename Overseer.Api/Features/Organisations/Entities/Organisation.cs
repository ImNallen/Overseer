using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Features.Organisations.Entities;

public sealed class Organisation : Entity
{
    private Organisation(
        Guid id,
        string name,
        string description,
        DateTime createdAt)
        : base(id)
    {
        Name = name;
        Description = description;
        CreatedAt = createdAt;
    }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public ICollection<User> Users { get; private set; } = [];

    public static Organisation Create(string name, string description, DateTime createdAt)
    {
        var organisation = new Organisation(
            Guid.NewGuid(),
            name,
            description,
            createdAt);

        return organisation;
    }
}
