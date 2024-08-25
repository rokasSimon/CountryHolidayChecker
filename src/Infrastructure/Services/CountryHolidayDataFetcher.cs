using System.Net.Http.Json;
using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Services.External;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class CountryHolidayDataFetcher(
    IHttpClientFactory _httpClientFactory,
    IOptions<CountryHolidayFetcherOptions> _options
    ) : ICountryHolidayDataFetcher
{
    public async Task<IEnumerable<FetchedCountryHoliday>> FetchHolidaysAsync(string countryCode, int year)
    {
        var client = _httpClientFactory.CreateClient(nameof(CountryHolidayFetcherOptions));

        var response = await client.GetFromJsonAsync<IEnumerable<FetchedCountryHoliday>>(_options.Value.CountryHolidayUrl);

        return response ?? [];
    }
}
