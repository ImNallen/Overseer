using System.Reflection;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using Overseer.Api.Abstractions.Behaviors;
using Overseer.Api.Abstractions.Encryption;
using Overseer.Api.Abstractions.Exceptions;
using Overseer.Api.Abstractions.Persistence;
using Overseer.Api.Abstractions.Time;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Persistence;
using Overseer.Api.Services.Encryption;
using Overseer.Api.Services.Outbox;
using Overseer.Api.Services.Time;
using Quartz;

namespace Overseer.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly assembly)
        => services
            .AddDatabase(configuration)
            .AddMediator()
            .AddFluentValidation(assembly)
            .AddSwagger()
            .AddHandlers()
            .AddServices()
            .AddCarter()
            .SetupQuartz(configuration)
            .AddEmail(configuration);

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("Database");
        Ensure.NotNullOrEmpty(connectionString);

        services.AddDbContext<OverseerDbContext>(
            options => options
                .UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default)));

        services.AddScoped<IUnitOfWork, OverseerDbContext>();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        return services;
    }

    private static IServiceCollection AddFluentValidation(
        this IServiceCollection services,
        Assembly assembly) =>
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }

    private static IServiceCollection SetupQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.Configure<OutboxOptions>(configuration.GetSection("Outbox"));
        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        return services;
    }

    private static IServiceCollection AddEmail(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddFluentEmail(configuration["Email:SenderEmail"], configuration["Email:SenderName"])
            .AddRazorRenderer()
            .AddSmtpSender(configuration["Email:Host"], configuration.GetValue<int>("Email:Port"));

        return services;
    }
}
