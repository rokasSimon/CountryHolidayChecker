using MediatR;

namespace Application.CountryHolidays.GroupedHolidays;

public class GetGroupedHolidaysHandler : IRequestHandler<GetGroupedHolidaysRequest, GetGroupedHolidaysResult>
{
    public Task<GetGroupedHolidaysResult> Handle(GetGroupedHolidaysRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}