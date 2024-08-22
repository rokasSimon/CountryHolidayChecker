using MediatR;

namespace Application.CountryHolidays.CountryList;

public class GetCountryListHandler : IRequestHandler<GetCountryListRequest, GetCountryListResult>
{
    public Task<GetCountryListResult> Handle(GetCountryListRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}