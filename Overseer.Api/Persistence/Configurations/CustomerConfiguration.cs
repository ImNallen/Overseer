using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overseer.Api.Features.Customers.Entities;
using Overseer.Api.Features.Shared;

namespace Overseer.Api.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(u => u.Email)
            .HasConversion(
                e => e.Value,
                v => Email.Create(v).Value)
            .HasColumnName(nameof(Email))
            .HasMaxLength(Email.MaxLength)
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

        builder.OwnsOne(c => c.Address);

        builder.HasIndex(c => c.Email)
            .IsUnique();
    }
}
