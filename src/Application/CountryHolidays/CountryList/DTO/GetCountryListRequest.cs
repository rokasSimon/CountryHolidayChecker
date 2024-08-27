using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Services.External;
using MediatR;

namespace Application.CountryHolidays.CountryList.DTO;

public sealed record GetCountryListRequest() : IRequest<GetCountryListResult>, IPreLoadable<CountryLoadData>
{
    public CountryLoadData GeneratePreLoadData()
    {
        return new CountryLoadData();
    }
}
