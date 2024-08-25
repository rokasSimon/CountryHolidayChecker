namespace Application.CountryHolidays.GroupedHolidays.DTO;

public record GetGroupedHolidaysResult(string CountryCode, int Year, IEnumerable<HolidayGroup> GroupedHolidays);

public record HolidayGroup(int Month, IEnumerable<Holiday> Holidays);

public record Holiday(int Day, IEnumerable<LocalizedName> Names);

public record LocalizedName(string Language, string Text);