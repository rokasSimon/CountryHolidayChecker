namespace Application.CountryHolidays.CountryList.DTO;

public record GetCountryListResult(IEnumerable<CountryRecord> SupportedCountries);

public record CountryRecord(string CountryCode, string CountryName);