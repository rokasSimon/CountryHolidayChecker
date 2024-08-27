using System.Globalization;

using Application.CountryHolidays.DayStatus.DTO;
using Application.Interfaces.Services.Utility;
using MediatR;

namespace Application.CountryHolidays.DayStatus.Handlers;

public class GetDayStatusHandler(IDayTypeChecker _dayTypeChecker)
    : IRequestHandler<GetDayStatusRequest, GetDayStatusResult>
{
    public async Task<GetDayStatusResult> Handle(GetDayStatusRequest request, CancellationToken cancellationToken)
    {
        var date = new DateOnly(request.Year, request.Month, request.Day);

        var dayStatus = await _dayTypeChecker.GetStatusForCountryDayAsync(date, request.CountryCode);

        return new GetDayStatusResult(date.ToString("yyyy-MM-dd"), dayStatus.ToString());
    }
}