using Application.CountryHolidays.MaximumFreeDays.DTO;
using Application.CountryHolidays.MaximumFreeDays.Handlers;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Utility;

using Domain.Entities;

using FluentAssertions;

using Moq;

namespace Application.UnitTests.CountryHolidays.Handlers;

[TestFixture]
public class GetMaximumFreeDaysHandlerTests
{
    private Mock<IHolidayRepository> _holidayRepository;
    private Mock<IFreeDayCalculator> _freeDayCalculator;

    [SetUp]
    public void Setup()
    {
        _holidayRepository = new Mock<IHolidayRepository>();
        _freeDayCalculator = new Mock<IFreeDayCalculator>();
    }

    [Test]
    public async Task Handle_WhenRepositoryHasHolidays_ShouldMapValidDateRange()
    {
        var countryCode = "ltu";
        var year = 2024;
        var expectedStartDate = new DateOnly(2024, 12, 24);
        var expectedEndDate = new DateOnly(2024, 12, 26);

        var holidays = new HolidayDate[]
        {
            new() { Date = expectedStartDate },
            new() { Date = new DateOnly(2024, 12, 25) },
            new() { Date = expectedEndDate },
        };
        var holidaySet = new HashSet<DateOnly>
        {
            expectedStartDate,
            new(2024, 12, 25),
            expectedEndDate,
        };

        _holidayRepository
            .Setup(x => x.GetAllHolidaysForCountryYearAsync(countryCode, year))
            .ReturnsAsync(holidays);
        _freeDayCalculator
            .Setup(x => x.FindMaximumRowOfFreeDays(year, holidaySet))
            .Returns((3, expectedStartDate, expectedEndDate));

        var handler = new GetMaximumFreeDaysHandler(_holidayRepository.Object, _freeDayCalculator.Object);
        var request = new GetMaximumFreeDaysRequest(countryCode, year);

        var result = await handler.Handle(request, new CancellationToken());

        result.Days.Should().Be(3);
        result.StartDate.Should().Be("2024-12-24");
        result.EndDate.Should().Be("2024-12-26");
    }
}
