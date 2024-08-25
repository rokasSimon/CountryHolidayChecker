namespace Application.CountryHolidays.DayStatus.DTO;

public record GetDayStatusResult(DateOnly date, DayType Status);

public enum DayType
{
    Workday,
    FreeDay,
    Holiday
}