namespace Infrastructure.Configurations;

public class CountryHolidayFetcherOptions
{
    public required string BaseUrl { get; init; }
    public required string CountryListUrl { get; init; }
    public required string CountryHolidayUrl { get; init; }
}
