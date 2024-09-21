using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features.Users;
using Overseer.Api.Services.Outbox;

namespace Overseer.Api.Abstractions.Persistence;

public interface IUnitOfWork
{
    DbSet<User> Users { get; }

    DbSet<OutboxMessage> OutboxMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
