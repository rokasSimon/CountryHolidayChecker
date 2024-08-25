using Application.CountryHolidays.CountryList.DTO;
using Application.Interfaces.Repositories;

using MediatR;

namespace Application.CountryHolidays.CountryList.Handlers;

public class GetCountryListHandler(ICountryRepository _countryRepository)
    : IRequestHandler<GetCountryListRequest, GetCountryListResult>
{
    public async Task<GetCountryListResult> Handle(GetCountryListRequest request, CancellationToken cancellationToken)
    {
        var countries = await _countryRepository.GetAllCountriesAsync();

        var mappedCountries = countries.Select(c => new CountryRecord(c.CountryCode, c.CountryName));

        return new GetCountryListResult(mappedCountries);
    }
}