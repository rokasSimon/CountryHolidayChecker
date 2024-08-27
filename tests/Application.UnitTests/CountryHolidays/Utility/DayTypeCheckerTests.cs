using Application.CountryHolidays.Common.DTO;
using Application.CountryHolidays.DayStatus.Utility;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Utility;

using Domain.Entities;

using FluentAssertions;

using Moq;

namespace Application.UnitTests.CountryHolidays.Utility;

[TestFixture]
public class DayTypeCheckerTests
{
    private Mock<IHolidayRepository> _holidayRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _holidayRepositoryMock = new Mock<IHolidayRepository>();
    }

    [Test]
    public async Task GetStatusForCountryDayAsync_WhenGivenValidHolidayDate_ShouldReturnHolidayStatus()
    {
        var date = new DateOnly(2024, 3, 11);
        var countryCode = "ltu";

        _holidayRepositoryMock
            .Setup(x => x.GetDateForCountryAsync(countryCode, date).Result)
            .Returns(new HolidayDate { Date = new DateOnly(2024, 3, 11) });
        var dayTypeChecker = new DayTypeChecker(_holidayRepositoryMock.Object);

        var result = await dayTypeChecker.GetStatusForCountryDayAsync(date, countryCode);

        result.Should().Be(DayType.Holiday);
    }

    [Test]
    public async Task GetStatusForCountryDayAsync_WhenGivenWeekendDate_ShouldReturnFreeDayStatus()
    {
        var date = new DateOnly(2024, 3, 10);
        var countryCode = "ltu";

        _holidayRepositoryMock
            .Setup(x => x.GetDateForCountryAsync(countryCode, date).Result)
            .Returns(value: null);
        var dayTypeChecker = new DayTypeChecker(_holidayRepositoryMock.Object);

        var result = await dayTypeChecker.GetStatusForCountryDayAsync(date, countryCode);

        result.Should().Be(DayType.FreeDay);
    }

    [Test]
    public async Task GetStatusForCountryDayAsync_WhenGivenWorkdayDate_ShouldReturnWorkdayStatus()
    {
        var date = new DateOnly(2024, 3, 8);
        var countryCode = "ltu";

        _holidayRepositoryMock
            .Setup(x => x.GetDateForCountryAsync(countryCode, date).Result)
            .Returns(value: null);
        var dayTypeChecker = new DayTypeChecker(_holidayRepositoryMock.Object);

        var result = await dayTypeChecker.GetStatusForCountryDayAsync(date, countryCode);

        result.Should().Be(DayType.Workday);
    }

    [Test]
    public async Task GetStatusForCountryDayAsync_WhenGivenOverlappingDate_ShouldReturnPrioritizedHolidayStatus()
    {
        var date = new DateOnly(2024, 3, 10);
        var countryCode = "ltu";

        _holidayRepositoryMock
            .Setup(x => x.GetDateForCountryAsync(countryCode, date).Result)
            .Returns(new HolidayDate { Date = new DateOnly(2024, 3, 10) });
        var dayTypeChecker = new DayTypeChecker(_holidayRepositoryMock.Object);

        var result = await dayTypeChecker.GetStatusForCountryDayAsync(date, countryCode);

        result.Should().Be(DayType.Holiday);
    }
}
