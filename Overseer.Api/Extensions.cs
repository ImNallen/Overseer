using Microsoft.EntityFrameworkCore;
using Overseer.Api.Persistence;

namespace Overseer.Api;

public static class Extensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using OverseerDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<OverseerDbContext>();

        dbContext.Database.Migrate();
    }
}
