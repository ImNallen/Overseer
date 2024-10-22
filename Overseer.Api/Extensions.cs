using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Microsoft.EntityFrameworkCore;
using Overseer.Api.Features;
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

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder? versionedGroupBuilder = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = versionedGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    public static RouteHandlerBuilder HasPermission(this RouteHandlerBuilder app, string permission) => app.RequireAuthorization(permission);
}
