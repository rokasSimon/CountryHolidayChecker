using System.Net.Http.Json;
using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Services.External;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class CountryDataFetcher(
    IHttpClientFactory _httpClientFactory,
    IOptions<CountryHolidayFetcherOptions> _options
    ) : ICountryDataFetcher
{
    public async Task<IEnumerable<FetchedCountry>> FetchCountriesAsync()
    {
        var client = _httpClientFactory.CreateClient(nameof(CountryHolidayFetcherOptions));

        var response = await client.GetFromJsonAsync<IEnumerable<FetchedCountry>>(_options.Value.CountryListUrl);

        return response ?? [];
    }
}
