using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overseer.Api.Features.Users;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Username)
            .HasMaxLength(50);

        builder.Property(u => u.Password)
            .HasMaxLength(255);

        builder.Property(u => u.FirstName)
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .HasMaxLength(50);

        builder.Property(u => u.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.HasIndex(u => u.InviteToken)
            .IsUnique();

        builder.HasIndex(u => u.PasswordResetToken)
            .IsUnique();

        builder.HasIndex(u => u.RefreshToken)
            .IsUnique();

        builder.HasIndex(u => u.EmailVerificationToken)
            .IsUnique();

        builder.HasOne(u => u.Organisation)
            .WithMany(o => o.Users)
            .HasForeignKey(u => u.OrganisationId);
    }
}
