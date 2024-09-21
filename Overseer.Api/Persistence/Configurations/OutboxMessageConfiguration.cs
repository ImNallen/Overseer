using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Overseer.Api.Services.Outbox;

namespace Overseer.Api.Persistence.Configurations;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("Outbox_Message");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Content)
            .HasMaxLength(4000)
            .HasColumnType("jsonb");
    }
}
