using System.Reflection;
using Carter;
using Overseer.Api;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddServices(
    builder.Configuration,
    Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapCarter();

app.UseExceptionHandler();

await app.RunAsync();
