using API.Infrastructure;
using Application.CountryHolidays.GroupedHolidays.DTO;
using MediatR;

namespace API.Endpoints;

public class GroupedHolidays : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("groupHolidays", async (ISender _mediator, [AsParameters] GetGroupedHolidaysRequest request) =>
        {
            return await _mediator.Send(request);
        })
        .WithName("groupHolidays")
        .WithOpenApi();
    }
}