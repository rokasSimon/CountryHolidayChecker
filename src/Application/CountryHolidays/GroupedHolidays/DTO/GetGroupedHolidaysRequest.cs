using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Services.External;
using MediatR;

namespace Application.CountryHolidays.GroupedHolidays.DTO;

public record GetGroupedHolidaysRequest(string CountryCode, int Year) : IRequest<GetGroupedHolidaysResult>, IPreLoadable<CountryHolidayLoadData>
{
    public CountryHolidayLoadData GeneratePreLoadData()
    {
        return new CountryHolidayLoadData(CountryCode, Year);
    }
}
