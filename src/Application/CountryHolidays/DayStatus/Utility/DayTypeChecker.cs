using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Utility;

namespace Application.CountryHolidays.DayStatus.Utility;

public class DayTypeChecker(IHolidayRepository _holidayRepository)
    : IDayTypeChecker
{
    public async Task<DayType> GetStatusForCountryDayAsync(DateOnly date, string countryCode)
    {
        var matchedHoliday = await _holidayRepository.GetDateForCountryAsync(countryCode, date);
        var isHoliday = matchedHoliday is not null;

        if (isHoliday) return DayType.Holiday;
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return DayType.FreeDay;

        return DayType.Workday;
    }
}
