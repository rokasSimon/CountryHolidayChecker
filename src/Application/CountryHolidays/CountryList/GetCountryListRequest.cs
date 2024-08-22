using MediatR;

namespace Application.CountryHolidays.CountryList;

public sealed record GetCountryListRequest() : IRequest<GetCountryListResult>;