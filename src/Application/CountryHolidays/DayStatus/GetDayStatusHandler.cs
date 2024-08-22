using MediatR;

namespace Application.CountryHolidays.DayStatus;

public class GetDayStatusHandler : IRequestHandler<GetDayStatusRequest, GetDayStatusResult>
{
    public Task<GetDayStatusResult> Handle(GetDayStatusRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}