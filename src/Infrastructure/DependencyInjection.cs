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
        var connectionString = configuration.GetConnectionString("CountryHolidayDB");

        services.AddDbContext<CountryHolidayContext>((_, options) =>
        {
            options.UseSqlServer(connectionString);
        });

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
