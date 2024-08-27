namespace Application.CountryHolidays.Common.DTO;

public record FetchedCountryHoliday(FetchedDate Date, IEnumerable<FetchedLocalizedName> Name);

public record FetchedDate(int Day, int Month, int Year, int DayOfWeek);
public record FetchedLocalizedName(string Lang, string Text);