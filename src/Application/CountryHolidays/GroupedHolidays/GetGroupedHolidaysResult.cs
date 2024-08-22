namespace Application.CountryHolidays.GroupedHolidays;

public record GetGroupedHolidaysResult(string CountryCode, int Year, IEnumerable<GroupedHolidays> GroupedHolidays);

public record GroupedHolidays(int Month, IEnumerable<Holiday> Holidays);

public record Holiday(int Day, int DayOfWeek, IEnumerable<LocalizedName> Names);

public record LocalizedName(string Language, string Text);