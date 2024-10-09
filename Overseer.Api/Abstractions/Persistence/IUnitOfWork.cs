using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features.Customers.Entities;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Services.Outbox;

namespace Overseer.Api.Abstractions.Persistence;

public interface IUnitOfWork
{
    DbSet<User> Users { get; }

    DbSet<Role> Roles { get; }

    DbSet<Permission> Permissions { get; }

    DbSet<RolePermission> RolePermissions { get; }

    DbSet<Customer> Customers { get; }

    DbSet<OutboxMessage> OutboxMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
