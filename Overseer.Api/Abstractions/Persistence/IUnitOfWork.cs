using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features.Products.Entities;
using Overseer.Api.Features.Users;
using Overseer.Api.Features.Users.Entities;
using Overseer.Api.Services.Outbox;

namespace Overseer.Api.Abstractions.Persistence;

public interface IUnitOfWork
{
    DbSet<User> Users { get; }

    DbSet<Role> Roles { get; }

    DbSet<Product> Products { get; }

    DbSet<OutboxMessage> OutboxMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
