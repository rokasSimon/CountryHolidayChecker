using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.External;

using Domain.Entities;

namespace Infrastructure.Services;

public class CountryDataLoaderStep(
    ICountryRepository _countryRepository,
    ICountryDataFetcher _countryFetcher
    ) : ILoadStep<CountryLoadData>
{
    public async Task LoadAsync(CountryLoadData request)
    {
        var fetchedCountries = await _countryFetcher.FetchCountriesAsync();
        var countries = fetchedCountries
            .Select(c => new Country { CountryCode = c.CountryCode, CountryName = c.FullName });
        
        await _countryRepository.AddCountriesAsync(countries);
    }

    public async Task<bool> ShouldLoadAsync(CountryLoadData request)
    {
        return await _countryRepository.IsEmptyAsync();
    }
}
