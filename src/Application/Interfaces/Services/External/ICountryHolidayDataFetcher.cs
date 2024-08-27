using Application.CountryHolidays.Common.DTO;

namespace Application.Interfaces.Services.External;

public interface ICountryHolidayDataFetcher
{
    Task<IEnumerable<FetchedCountryHoliday>> FetchHolidaysAsync(string countryCode, int year);
}
