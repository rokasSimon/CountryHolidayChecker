using Application.CountryHolidays.Common.DTO;

namespace Application.Interfaces.Services.Utility;

public interface IDayTypeChecker
{
    Task<DayType> GetStatusForCountryDayAsync(DateOnly date, string countryCode);
}
