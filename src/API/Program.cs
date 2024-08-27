using API;
using Application;
using Infrastructure;
using Infrastructure.Contexts;

using Microsoft.EntityFrameworkCore;

using Migrations.Sqlite;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddAPIServices();

var sqliteConnectionString = builder.Configuration.GetConnectionString("CountryHolidaySqliteDB");

if (sqliteConnectionString != null)
{
    builder.Services.AddDbContext<CountryHolidayContext>((_, options) =>
    {
        options.UseSqlite(sqliteConnectionString,
                    opt => opt.MigrationsAssembly(typeof(Marker).Assembly.GetName().Name));
    });
}

var app = builder.Build();

if (sqliteConnectionString != null)
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<CountryHolidayContext>();

    await service.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseExceptionHandler();

var routeGroup = app.MapGroup("api/");
app.MapEndpoints(routeGroup);

app.Run();