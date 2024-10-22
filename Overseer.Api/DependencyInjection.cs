using System.Reflection;
using System.Text;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Overseer.Api.Features;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Persistence;
using Overseer.Api.Services.Authentication;
using Overseer.Api.Services.Authorization;
using Overseer.Api.Services.Encryption;
using Overseer.Api.Services.Outbox;
using Overseer.Api.Services.Time;
using Overseer.Api.Utilities.Behaviors;
using Overseer.Api.Utilities.Exceptions;
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
            .AddEndpoints(assembly)
            .AddApiVersioning()
            .AddHandlers()
            .AddServices()
            .SetupQuartz(configuration)
            .AddEmail(configuration)
            .AddAuth(configuration);

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

    private static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

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

    private static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IPasswordGenerator, PasswordGenerator>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        IConfigurationSection jwtSettings = configuration.GetSection("JwtSettings");
        byte[] key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false; // Set to true in production
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }

    private static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }
}
