using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overseer.Api.Features.Shared;
using Overseer.Api.Features.Users;
using Overseer.Api.Features.Users.Entities;

namespace Overseer.Api.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .HasConversion(
                e => e.Value,
                v => Email.Create(v).Value)
            .HasColumnName(nameof(Email))
            .HasMaxLength(Email.MaxLength)
            .IsRequired();

        builder.Property(u => u.Username)
            .HasConversion(
                e => e.Value,
                v => Username.Create(v).Value)
            .HasColumnName(nameof(Username))
            .HasMaxLength(Username.MaxLength)
            .IsRequired();

        builder.Property(u => u.Password)
            .HasConversion(
                p => p.Value,
                v => Password.Create(v).Value)
            .HasColumnName(nameof(Password))
            .HasMaxLength(Password.MaxLength)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasConversion(
                e => e.Value,
                v => FirstName.Create(v).Value)
            .HasColumnName(nameof(FirstName))
            .HasMaxLength(FirstName.MaxLength)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasConversion(
                e => e.Value,
                v => LastName.Create(v).Value)
            .HasColumnName(nameof(LastName))
            .HasMaxLength(LastName.MaxLength)
            .IsRequired();

        builder.Property(u => u.RefreshToken)
            .HasConversion(
                e => e!.Value,
                v => RefreshToken.Create(v).Value)
            .HasColumnName(nameof(RefreshToken))
            .HasMaxLength(100);

        builder.Property(u => u.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.HasIndex(u => u.PasswordResetToken)
            .IsUnique();

        builder.HasIndex(u => u.RefreshToken)
            .IsUnique();

        builder.HasIndex(u => u.EmailVerificationToken)
            .IsUnique();
    }
}
