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

                return new HolidayGroup(MonthToString(gr.Key), holidays);
            });

        var d = new DateOnly(1, 1, 1);

        return new GetGroupedHolidaysResult(request.CountryCode, request.Year, groupedHolidays);
    }

    private static string MonthToString(int month) => month switch
    {
        1 => "January",
        2 => "February",
        3 => "March",
        4 => "April",
        5 => "May",
        6 => "June",
        7 => "July",
        8 => "August",
        9 => "September",
        10 => "October",
        11 => "November",
        12 => "December",
        _ => throw new ArgumentException("Invalid month passed as an argument")
    };
}