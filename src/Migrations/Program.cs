using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = Host.CreateDefaultBuilder(args);
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
    var connectionString = Environment.GetEnvironmentVariable("MSSQL_CONNECTION_STRING")
        ?? throw new ArgumentException("Database connection string not specified in environment variables");

    builder.UseEnvironment(environment);
    builder.ConfigureAppConfiguration((context, builder) =>
    {
        context.Configuration["ConnectionStrings:CountryHolidayDB"] = connectionString;
    });

    builder.ConfigureServices((context, services) =>
    {
        services.AddSerilog();
        services.AddInfrastructureServices(context.Configuration);
    });

    var app = builder.Build();

    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<CountryHolidayContext>();

    await service.Database.MigrateAsync();

    return 0;
}
catch (Exception e)
{
    Log.Fatal(e, "Could not complete migration because of exception:");

    return 1;
}