using Application.CountryHolidays.Common.DTO;
using Application.CountryHolidays.DayStatus.DTO;
using Application.CountryHolidays.DayStatus.Handlers;
using Application.Interfaces.Services.Utility;

using FluentAssertions;

using Moq;

namespace Application.UnitTests.CountryHolidays.Handlers;

[TestFixture]
public class GetDayStatusHandlerTests
{
    private Mock<IDayTypeChecker> _dayTypeChecker;

    [SetUp]
    public void Setup()
    {
        _dayTypeChecker = new Mock<IDayTypeChecker>();
    }

    [Test]
    public async Task Handle_WhenRepositoryHasHolidays_ShouldMapValidDateStringAndStatus()
    {
        var date = new DateOnly(2024, 3, 11);
        _dayTypeChecker
            .Setup(x => x.GetStatusForCountryDayAsync(date, "ltu"))
            .ReturnsAsync(DayType.Holiday);

        var handler = new GetDayStatusHandler(_dayTypeChecker.Object);
        var request = new GetDayStatusRequest(2024, 3, 11, "ltu");

        var result = await handler.Handle(request, new CancellationToken());

        result.Status.Should().Be("Holiday");
        result.Date.Should().Be("2024-03-11");
    }
}
