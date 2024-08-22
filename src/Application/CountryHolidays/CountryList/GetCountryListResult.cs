namespace Application.CountryHolidays.CountryList;

public record GetCountryListResult(IEnumerable<CountryRecord> SupportedCountries);

public record CountryRecord(string CountryCode, string CountryName);