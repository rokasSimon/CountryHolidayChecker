using MediatR;

namespace Application.CountryHolidays.MaximumFreeDays;

public record GetMaximumFreeDaysRequest(string CountryCode, int Year) : IRequest<GetMaximumFreeDaysResult>;