using Application.CountryHolidays.Common.DTO;

namespace Application.Interfaces.Services.External;

public interface ICountryDataFetcher
{
    Task<IEnumerable<FetchedCountry>> FetchCountriesAsync();
}
