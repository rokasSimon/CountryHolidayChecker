using Application.CountryHolidays.MaximumFreeDays.Utility;
using Application.Interfaces.Services.Utility;

using FluentAssertions;

namespace Application.UnitTests.CountryHolidays.Utility;

[TestFixture]
public class FreeDayCalculatorTests
{
    private readonly IFreeDayCalculator _freeDayCalculator;

    public FreeDayCalculatorTests()
    {
        _freeDayCalculator = new FreeDayCalculator();
    }

    [Test]
    public void FindMaximumRowOfFreeDays_WhenHolidayIsBeforeNewYear_ShouldEndThisYear()
    {
        var year = 2022;
        var holidays = new HashSet<DateOnly>
        {
            new(year, 12, 30), // Friday
            new(year, 12, 31), // Saturday
        };

        var result = _freeDayCalculator.FindMaximumRowOfFreeDays(year, holidays);

        result.days.Should().Be(2);
        result.startDate.Should().Be(new DateOnly(2022, 12, 30));
        result.endDate.Should().Be(new DateOnly(2022, 12, 31));
    }

    [Test]
    public void FindMaximumRowOfFreeDays_WhenNoHolidays_ShouldBeFirstWeekend()
    {
        var year = 2022;
        var holidays = new HashSet<DateOnly>();

        var result = _freeDayCalculator.FindMaximumRowOfFreeDays(year, holidays);

        result.days.Should().Be(2);
        result.startDate.Should().Be(new DateOnly(2022, 1, 1));
        result.endDate.Should().Be(new DateOnly(2022, 1, 2));
    }

    [Test]
    public void FindMaximumRowOfFreeDays_WhenHolidaysLastFullWeekWorkdays_ShouldLastNineDays()
    {
        var year = 2024;
        var holidays = new HashSet<DateOnly>
        {
            new(year, 1, 8), // Monday
            new(year, 1, 9), // Tuesday
            new(year, 1, 10), // ...
            new(year, 1, 11),
            new(year, 1, 12)
        };

        var result = _freeDayCalculator.FindMaximumRowOfFreeDays(year, holidays);

        result.days.Should().Be(9);
        result.startDate.Should().Be(new DateOnly(2024, 1, 6));
        result.endDate.Should().Be(new DateOnly(2024, 1, 14));
    }

    [Test]
    public void FindMaximumRowOfFreeDays_WhenHolidaysAreSeparated_ShouldFindLongestRow()
    {
        var year = 2024;
        var holidays = new HashSet<DateOnly>
        {
            new(year, 1, 10), // Wednesday
            new(year, 1, 11), // Thursday
            new(year, 5, 16), // Thursday
            new(year, 5, 17), // Friday
            new(year, 5, 20), // Monday
        };

        var result = _freeDayCalculator.FindMaximumRowOfFreeDays(year, holidays);

        result.days.Should().Be(5);
        result.startDate.Should().Be(new DateOnly(2024, 5, 16));
        result.endDate.Should().Be(new DateOnly(2024, 5, 20));
    }
}
