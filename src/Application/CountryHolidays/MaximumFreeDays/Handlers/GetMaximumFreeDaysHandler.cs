using System.Globalization;

using Application.CountryHolidays.MaximumFreeDays.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Utility;
using MediatR;

namespace Application.CountryHolidays.MaximumFreeDays.Handlers;

public class GetMaximumFreeDaysHandler(
    IHolidayRepository _holidayRepository,
    IFreeDayCalculator _freeDayCalculator)
    : IRequestHandler<GetMaximumFreeDaysRequest, GetMaximumFreeDaysResult>
{
    public async Task<GetMaximumFreeDaysResult> Handle(GetMaximumFreeDaysRequest request, CancellationToken cancellationToken)
    {
        var holidays = await _holidayRepository.GetAllHolidaysForCountryYearAsync(request.CountryCode, request.Year);

        var (days, startDate, endDate) = _freeDayCalculator.FindMaximumRowOfFreeDays(request.Year, holidays.Select(x => x.Date).ToHashSet());

        return new GetMaximumFreeDaysResult(
            days,
            startDate.ToString("yyyy-MM-dd"),
            endDate.ToString("yyyy-MM-dd"));
    }
}