namespace Overseer.Api.Features.Users.Entities;

public class Permission
{
    public static readonly Permission UsersRead = new(1, "users:read");

    public static readonly Permission UsersWrite = new(2, "users:write");

    public Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public ICollection<RolePermission> RolePermissions { get; init; } = new List<RolePermission>();
}
