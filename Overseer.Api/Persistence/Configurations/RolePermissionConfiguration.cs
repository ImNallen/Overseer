using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder
            .HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        builder
            .HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        builder.HasData(
            new RolePermission { RoleId = Role.Admin.Id, PermissionId = Permission.UsersRead.Id },
            new RolePermission { RoleId = Role.Admin.Id, PermissionId = Permission.UsersWrite.Id },
            new RolePermission { RoleId = Role.Admin.Id, PermissionId = Permission.UsersDelete.Id },
            new RolePermission { RoleId = Role.Admin.Id, PermissionId = Permission.Admin.Id },
            new RolePermission { RoleId = Role.Admin.Id, PermissionId = Permission.CustomersRead.Id },
            new RolePermission { RoleId = Role.Admin.Id, PermissionId = Permission.CustomersWrite.Id },
            new RolePermission { RoleId = Role.User.Id, PermissionId = Permission.UsersRead.Id },
            new RolePermission { RoleId = Role.User.Id, PermissionId = Permission.UsersWrite.Id },
            new RolePermission { RoleId = Role.User.Id, PermissionId = Permission.UsersDelete.Id },
            new RolePermission { RoleId = Role.User.Id, PermissionId = Permission.CustomersRead.Id },
            new RolePermission { RoleId = Role.User.Id, PermissionId = Permission.CustomersWrite.Id });
    }
}
