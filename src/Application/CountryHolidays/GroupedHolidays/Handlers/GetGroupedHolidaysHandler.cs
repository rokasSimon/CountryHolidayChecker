using Application.CountryHolidays.GroupedHolidays.DTO;
using Application.Interfaces.Repositories;

using MediatR;

namespace Application.CountryHolidays.GroupedHolidays.Handlers;

public class GetGroupedHolidaysHandler(IHolidayRepository _holidayRepository)
    : IRequestHandler<GetGroupedHolidaysRequest, GetGroupedHolidaysResult>
{
    public async Task<GetGroupedHolidaysResult> Handle(GetGroupedHolidaysRequest request, CancellationToken cancellationToken)
    {
        var countryHolidays = await _holidayRepository.GetAllHolidaysForCountryYearAsync(request.CountryCode, request.Year);

        var groupedHolidays = countryHolidays
            .GroupBy(h => h.Date.Month)
            .Select(gr =>
            {
                var holidays = gr.Select(h =>
                    new Holiday(h.Date.Day, h.Holiday.Names.Select(n =>
                        new LocalizedName(n.Language, n.Text))));

                return new HolidayGroup(gr.Key, holidays);
            });

        return new GetGroupedHolidaysResult(request.CountryCode, request.Year, groupedHolidays);
    }
}