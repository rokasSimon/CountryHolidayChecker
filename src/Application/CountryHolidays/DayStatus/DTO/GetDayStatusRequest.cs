using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Services.External;
using MediatR;

namespace Application.CountryHolidays.DayStatus.DTO;

public sealed record GetDayStatusRequest(int Year, int Month, int Day, string CountryCode) : IRequest<GetDayStatusResult>, IPreLoadable<CountryHolidayLoadData>
{
    public CountryHolidayLoadData GeneratePreLoadData()
    {
        return new CountryHolidayLoadData(CountryCode, Year);
    }
}
