using API.Infrastructure;
using Application.CountryHolidays.CountryList;
using MediatR;

namespace API.Endpoints;

public class CountryList : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("supportedCountries", async (ISender _mediator) =>
        {
            return await _mediator.Send(new GetCountryListRequest());
        })
        .WithName("SupportedCountries")
        .WithOpenApi();
    }
}