using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);

        builder
            .HasMany(p => p.RolePermissions)
            .WithOne()
            .HasForeignKey(rp => rp.PermissionId);

        builder.HasData(Permission.UsersRead, Permission.UsersWrite);
    }
}
