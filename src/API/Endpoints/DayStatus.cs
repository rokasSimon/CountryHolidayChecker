using API.Infrastructure;
using Application.CountryHolidays.DayStatus.DTO;
using MediatR;

namespace API.Endpoints;

public class DayStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("dayStatus", async (ISender _mediator, [AsParameters] GetDayStatusRequest request) =>
        {
            return await _mediator.Send(request);
        })
        .WithName("DayStatus")
        .WithOpenApi();
    }
}
