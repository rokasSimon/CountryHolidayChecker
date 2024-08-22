namespace Application.CountryHolidays.MaximumFreeDays;

public record GetMaximumFreeDaysResult(int Year, int Days, int StartMonth, int StartDay, int EndMonth, int EndDay);