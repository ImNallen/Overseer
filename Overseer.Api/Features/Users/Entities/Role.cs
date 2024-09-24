namespace Overseer.Api.Features.Users.Entities;

public sealed class Role
{
    public static readonly Role Admin = new(1, "Admin");

    public static readonly Role User = new(2, "User");

    public static readonly Role ReadOnly = new(3, "ReadOnly");

    public static readonly List<Role> All = [Admin, User, ReadOnly];

    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public ICollection<User> Users { get; init; } = new List<User>();

    public ICollection<RolePermission> RolePermissions { get; init; } = new List<RolePermission>();
}
