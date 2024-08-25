namespace Application.CountryHolidays.MaximumFreeDays.DTO;

public record GetMaximumFreeDaysResult(int days, DateOnly start, DateOnly end);