using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Services.External;
using MediatR;

namespace Application.CountryHolidays.DayStatus.DTO;

public sealed record GetDayStatusRequest(DateOnly Date, string CountryCode) : IRequest<GetDayStatusResult>, IPreLoadable<CountryHolidayLoadData>
{
    public CountryHolidayLoadData GeneratePreLoadData()
    {
        return new CountryHolidayLoadData(CountryCode, Date.Year);
    }
}
