using MediatR;

namespace Application.CountryHolidays.DayStatus;

public sealed record GetDayStatusRequest(int Year, int Month, int Day, string CountryCode) : IRequest<GetDayStatusResult>;