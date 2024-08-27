using Application.CountryHolidays.Common.DTO;
using Application.CountryHolidays.DayStatus.DTO;
using Application.CountryHolidays.DayStatus.Handlers;
using Application.CountryHolidays.GroupedHolidays.DTO;
using Application.CountryHolidays.GroupedHolidays.Handlers;
using Application.Interfaces.Repositories;

using Domain.Entities;

using FluentAssertions;

using Moq;

namespace Application.UnitTests.CountryHolidays.Handlers;

[TestFixture]
public class GetGroupedHolidaysHandlerTests
{
    private Mock<IHolidayRepository> _holidayRepository;

    [SetUp]
    public void Setup()
    {
        _holidayRepository = new Mock<IHolidayRepository>();
    }

    [Test]
    public async Task Handle_WhenRepositoryHasHolidays_ShouldMapValidHolidayGroup()
    {
        var countryCode = "ltu";
        var year = 2024;

        _holidayRepository
            .Setup(x => x.GetAllHolidaysForCountryYearAsync(countryCode, year))
            .ReturnsAsync([
                new HolidayDate
                {
                    Date = new DateOnly(year, 3, 11),
                    Holiday = new Domain.Entities.Holiday
                    {
                        Names = [ new Domain.Entities.LocalizedName { Language = "lt", Text = "Lietuvos nepriklausomybes atkurimo diena" } ],
                    },
                },
                new HolidayDate
                {
                    Date = new DateOnly(year, 3, 14),
                    Holiday = new Domain.Entities.Holiday
                    {
                        Names = [ new Domain.Entities.LocalizedName { Language = "lt", Text = "Pi Day" } ],
                    },
                }
            ]);

        var handler = new GetGroupedHolidaysHandler(_holidayRepository.Object);
        var request = new GetGroupedHolidaysRequest(countryCode, year);

        var result = await handler.Handle(request, new CancellationToken());

        result.Year.Should().Be(year);
        result.CountryCode.Should().Be(countryCode);
        result.GroupedHolidays.Should().Contain(x => x.MonthName == "March" && x.Holidays.Count() == 2);
    }
}
