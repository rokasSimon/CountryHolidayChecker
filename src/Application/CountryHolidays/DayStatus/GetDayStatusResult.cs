namespace Application.CountryHolidays.DayStatus;

public record GetDayStatusResult(int Year, int Month, int Day, int DayOfWeek, string Status);