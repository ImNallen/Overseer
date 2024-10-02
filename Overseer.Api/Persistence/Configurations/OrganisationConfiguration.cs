using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overseer.Api.Features.Organisations.Entities;

namespace Overseer.Api.Persistence.Configurations;

public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(o => o.Description)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasMany(o => o.Users)
            .WithOne(u => u.Organisation)
            .HasForeignKey(u => u.OrganisationId);
    }
}
