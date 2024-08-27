using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.External;

using Infrastructure.Configurations;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var mssqlConnectionString = configuration.GetConnectionString("CountryHolidayDB");
        var sqliteConnectionString = configuration.GetConnectionString("CountryHolidaySqliteDB");

        // Had to be moved to API Assembly because we need reference to Sqlite migrations assembly,
        // but that assembly needs to reference the context assembly (this assembly)
        // so we get cyclical reference

        //services.AddDbContext<CountryHolidayContext>((_, options) =>
        //{
        //    if (sqliteConnectionString != null)
        //    {
        //        options.UseSqlite(sqliteConnectionString,
        //            opt => opt.MigrationsAssembly(typeof(Marker).Assembly.GetName().Name));
        //    }
        //    else
        //    {
        //        options.UseSqlServer(mssqlConnectionString,
        //            opt => opt.MigrationsAssembly(typeof(DependencyInjection).Assembly.GetName().Name));
        //    }
        //});

        if (sqliteConnectionString == null)
        {
            services.AddDbContext<CountryHolidayContext>((_, options) =>
            {
                options.UseSqlServer(mssqlConnectionString,
                    opt => opt.MigrationsAssembly(typeof(DependencyInjection).Assembly.GetName().Name));
            });
        }

        services.Configure<CountryHolidayFetcherOptions>(configuration.GetSection(nameof(CountryHolidayFetcherOptions)));

        services.AddHttpClient(nameof(CountryHolidayFetcherOptions), (sp, client) =>
        {
            var settings = sp
                .GetRequiredService<IOptions<CountryHolidayFetcherOptions>>().Value;

            client.BaseAddress = new Uri(settings.BaseUrl);
        });

        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IHolidayRepository, HolidayRepository>();

        services.AddScoped<ILoadStep<CountryLoadData>, CountryDataLoaderStep>();
        services.AddScoped<ILoadStep<CountryHolidayLoadData>, CountryHolidayDataLoaderStep>();

        services.AddScoped<ICountryDataFetcher, CountryDataFetcher>();
        services.AddScoped<ICountryHolidayDataFetcher, CountryHolidayDataFetcher>();

        return services;
    }
}
