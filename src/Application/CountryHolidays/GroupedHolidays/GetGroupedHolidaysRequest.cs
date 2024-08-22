using MediatR;

namespace Application.CountryHolidays.GroupedHolidays;

public record GetGroupedHolidaysRequest(string CountryCode, int Year) : IRequest<GetGroupedHolidaysResult>;