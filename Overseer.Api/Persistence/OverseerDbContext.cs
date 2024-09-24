using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Features.Products.Entities;
using Overseer.Api.Features.Users;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Services.Outbox;
using Overseer.Api.Services.Serialization;

namespace Overseer.Api.Persistence;

public class OverseerDbContext(
    DbContextOptions<OverseerDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        InsertDomainEvents(this);

        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OverseerDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }

    private static void InsertDomainEvents(DbContext context)
    {
        var domainEvents = context
            .ChangeTracker
            .Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(x =>
            {
                IReadOnlyList<IDomainEvent> domainEvents = x.GetDomainEvents();

                x.ClearDomainEvents();

                return domainEvents;
            })
            .Select(d => new OutboxMessage
            {
                Id = d.Id,
                Type = d.GetType().Name,
                Content = JsonConvert.SerializeObject(d, SerializerSettings.Instance),
                OccurredOnUtc = d.OccurredOnUtc,
            })
            .ToList();

        context.Set<OutboxMessage>().AddRange(domainEvents);
    }
}
