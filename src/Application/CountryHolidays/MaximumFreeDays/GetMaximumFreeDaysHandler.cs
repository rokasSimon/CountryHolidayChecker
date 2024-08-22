using MediatR;
namespace Application.CountryHolidays.MaximumFreeDays;

public class GetMaximumFreeDaysHandler : IRequestHandler<GetMaximumFreeDaysRequest, GetMaximumFreeDaysResult>
{
    public Task<GetMaximumFreeDaysResult> Handle(GetMaximumFreeDaysRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}