using Application.Interfaces.Services.Utility;

using Domain.Entities;

namespace Application.CountryHolidays.MaximumFreeDays.Utility;

public class FreeDayCalculator : IFreeDayCalculator
{
    public (int days, DateOnly startDate, DateOnly endDate) FindMaximumRowOfFreeDays(int year, IReadOnlySet<DateOnly> holidayDates)
    {
        // Possible minimum is 2 days from weekends, assuming all weekends are free days
        var (startDate, endDate) = FindFirstWeekend(year);

        foreach (var holidayDate in holidayDates)
        {
            var leftBound = holidayDate;
            while (leftBound.AddDays(-1).Year == year
                && IsFreeDay(leftBound.AddDays(-1), holidayDates))
            {
                leftBound = leftBound.AddDays(-1);
            }

            var rightBound = holidayDate;
            while (rightBound.AddDays(1).Year == year
                && IsFreeDay(rightBound.AddDays(1), holidayDates))
            {
                rightBound = rightBound.AddDays(1);
            }

            if (rightBound.DayNumber - leftBound.DayNumber >= endDate.DayNumber - startDate.DayNumber)
            {
                startDate = leftBound;
                endDate = rightBound;
            }
        }

        return (endDate.DayNumber - startDate.DayNumber + 1, startDate, endDate);
    }

    private (DateOnly firstWeekendStart, DateOnly firstWeekendEnd) FindFirstWeekend(int year)
    {
        var date = new DateOnly(year, 1, 1);

        while (date.DayOfWeek != DayOfWeek.Saturday)
        {
            date = date.AddDays(1);
        }

        return (date, date.AddDays(1));
    }

    private static bool IsFreeDay(DateOnly date, IReadOnlySet<DateOnly> holidayDates)
        => holidayDates.Contains(date)
        || date.DayOfWeek == DayOfWeek.Saturday
        || date.DayOfWeek == DayOfWeek.Sunday;
}
