using API.Infrastructure;
using Application.CountryHolidays.MaximumFreeDays;
using MediatR;

namespace API.Endpoints;

public class MaximumFreeDays : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("maximumFreeDays", async (ISender _mediator, [AsParameters] GetMaximumFreeDaysRequest request) =>
        {
            return await _mediator.Send(request);
        })
        .WithName("MaximumFreeDays")
        .WithOpenApi();
    }
}