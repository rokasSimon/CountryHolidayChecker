using System.Net.Http.Json;

using Application.CountryHolidays.CountryList.DTO;
using Application.CountryHolidays.DayStatus.DTO;
using Application.CountryHolidays.GroupedHolidays.DTO;
using Application.CountryHolidays.MaximumFreeDays.DTO;

using FluentAssertions;

namespace API.SmokeTests;

[TestFixture]
public class APISmokeTests
{
    private HttpClient _client = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:8081/api/")
        };
    }

    [Test]
    public async Task GetCountryList_WhenGivenNothing_ShouldReturnAFilledListOfCountriesContainingLithuania()
    {
        var url = "supportedCountries";

        var response = await _client.GetFromJsonAsync<GetCountryListResult>(url);

        response.Should().NotBeNull();
        response.SupportedCountries.Should().NotBeEmpty();
        response.SupportedCountries.Should().Contain(x => x.CountryCode == "ltu" && x.CountryName == "Lithuania");
    }

    [Test]
    public async Task GetDayStatus_WhenGivenAValidLithuanianHoliday_ShouldReturnAHolidayStatus()
    {
        var expectedDateString = "2024-03-11";
        var expectedStatus = "Holiday";

        var date = new DateOnly(2024, 3, 11);
        var countryCode = "ltu";
        var url = $"dayStatus?year={date.Year}&month={date.Month}&day={date.Day}&countryCode={countryCode}";

        var response = await _client.GetFromJsonAsync<GetDayStatusResult>(url);

        response.Should().NotBeNull();
        response.Date.Should().Be(expectedDateString);
        response.Status.Should().Be(expectedStatus);
    }

    [Test]
    public async Task GetGroupedHolidays_WhenGivenAValidLithuanianHoliday_ShouldReturnAHolidayStatus()
    {
        var expectedCountryCode = "ltu";
        var expectedYear = 2024;

        var year = 2024;
        var countryCode = "ltu";
        var url = $"groupHolidays?year={year}&countryCode={countryCode}";

        var response = await _client.GetFromJsonAsync<GetGroupedHolidaysResult>(url);

        response.Should().NotBeNull();
        response.Year.Should().Be(expectedYear);
        response.CountryCode.Should().Be(expectedCountryCode);
        response.GroupedHolidays.Should().HaveCount(10);
        response.GroupedHolidays.Last().Holidays.Should().HaveCount(3); // Three Christmas days in December
    }

    [Test]
    public async Task GetMaximumFreeDays_WhenGivenLithuania2024_ShouldReturnAThreeDayRange()
    {
        var expectedDays = 3;
        var expectedStartDate = "2024-12-24";
        var expectedEndDate = "2024-12-26";

        var year = 2024;
        var countryCode = "ltu";
        var url = $"maximumFreeDays?year={year}&countryCode={countryCode}";

        var response = await _client.GetFromJsonAsync<GetMaximumFreeDaysResult>(url);

        response.Should().NotBeNull();
        response.Days.Should().Be(expectedDays);
        response.StartDate.Should().Be(expectedStartDate);
        response.EndDate.Should().Be(expectedEndDate);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
    }
}
