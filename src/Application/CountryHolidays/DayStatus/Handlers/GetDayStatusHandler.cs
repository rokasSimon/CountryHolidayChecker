using Application.CountryHolidays.CountryList.DTO;
using Application.CountryHolidays.DayStatus.DTO;
using Application.Interfaces.Repositories;

using MediatR;

namespace Application.CountryHolidays.DayStatus.Handlers;

public class GetDayStatusHandler(IHolidayRepository _holidayRepository)
    : IRequestHandler<GetDayStatusRequest, GetDayStatusResult>
{
    public async Task<GetDayStatusResult> Handle(GetDayStatusRequest request, CancellationToken cancellationToken)
    {
        // TODO: Move this function out into interface
        var dayStatus = await GetDayStatusForCountryDate(request.Date, request.CountryCode);

        return new GetDayStatusResult(request.Date, dayStatus);
    }

    private async Task<DayType> GetDayStatusForCountryDate(DateOnly date, string countryCode)
    {
        var matchedHoliday = await _holidayRepository.GetDateForCountryAsync(countryCode, date);
        var isHoliday = matchedHoliday is not null;

        if (isHoliday) return DayType.Holiday;
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return DayType.FreeDay;

        return DayType.Workday;
    }
}