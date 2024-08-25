using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Services.External;
using MediatR;

namespace Application.CountryHolidays.MaximumFreeDays.DTO;

public record GetMaximumFreeDaysRequest(string CountryCode, int Year) : IRequest<GetMaximumFreeDaysResult>, IPreLoadable<CountryHolidayLoadData>
{
    public CountryHolidayLoadData GeneratePreLoadData()
    {
        return new CountryHolidayLoadData(CountryCode, Year);
    }
}
