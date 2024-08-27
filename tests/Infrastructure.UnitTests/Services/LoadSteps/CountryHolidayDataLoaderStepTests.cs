using Application.CountryHolidays.Common.DTO;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services.External;

using Domain.Entities;
using FluentAssertions;
using Infrastructure.Common.Exceptions;
using Infrastructure.Services;

using Moq;

namespace Infrastructure.UnitTests.Services.LoadSteps;

[TestFixture]
public class CountryHolidayDataLoaderStepTests
{
    private Mock<ICountryRepository> _countryRepository;
    private Mock<IHolidayRepository> _holidayRepository;
    private Mock<ICountryHolidayDataFetcher> _holidayFetcher;
    private Mock<ILoadStep<CountryLoadData>> _countryLoadStep;

    [SetUp]
    public void Setup()
    {
        _countryRepository = new Mock<ICountryRepository>();
        _holidayRepository = new Mock<IHolidayRepository>();
        _holidayFetcher = new Mock<ICountryHolidayDataFetcher>();
        _countryLoadStep = new Mock<ILoadStep<CountryLoadData>>();
    }

    [Test]
    public async Task ShouldLoadAsync_WhenHolidayRepositoryIsEmpty_ShouldReturnTrue()
    {
        _holidayRepository
            .Setup(x => x.GetAllHolidaysForCountryYearAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync([]);

        var loader = new CountryHolidayDataLoaderStep(
            _countryRepository.Object,
            _holidayRepository.Object,
            _holidayFetcher.Object,
            _countryLoadStep.Object);
        var request = new CountryHolidayLoadData(It.IsAny<string>(), It.IsAny<int>());

        var result = await loader.ShouldLoadAsync(request);

        result.Should().Be(true);
    }

    [Test]
    public async Task ShouldLoadAsync_WhenHolidayRepositoryIsNotEmpty_ShouldReturnFalse()
    {
        _holidayRepository
            .Setup(x => x.GetAllHolidaysForCountryYearAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync([ new HolidayDate { Date = new DateOnly(2024, 3, 11) } ]);

        var loader = new CountryHolidayDataLoaderStep(
            _countryRepository.Object,
            _holidayRepository.Object,
            _holidayFetcher.Object,
            _countryLoadStep.Object);
        var request = new CountryHolidayLoadData(It.IsAny<string>(), 2024);

        var result = await loader.ShouldLoadAsync(request);

        result.Should().Be(false);
    }

    [Test]
    public async Task LoadAsync_WhenCountriesNotLoaded_ShouldLoadCountries()
    {
        _countryLoadStep
            .Setup(x => x.ShouldLoadAsync(new CountryLoadData()))
            .ReturnsAsync(true);
        _countryRepository
            .Setup(x => x.GetCountryByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new Country { CountryCode = "test", CountryName = "test" });

        var loader = new CountryHolidayDataLoaderStep(
            _countryRepository.Object,
            _holidayRepository.Object,
            _holidayFetcher.Object,
            _countryLoadStep.Object);
        var request = new CountryHolidayLoadData(It.IsAny<string>(), It.IsAny<int>());

        await loader.LoadAsync(request);

        _countryLoadStep.Verify(x => x.LoadAsync(new CountryLoadData()), Times.Once);
    }

    [Test]
    public async Task LoadAsync_WhenHolidayServiceIsNotAvailable_ShouldThrowFailedFetchException()
    {
        _holidayFetcher
            .Setup(x => x.FetchHolidaysAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ThrowsAsync(new FailedFetchException(It.IsAny<string>()));
        _countryRepository
            .Setup(x => x.GetCountryByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(new Country { CountryCode = "test", CountryName = "test" });

        var loader = new CountryHolidayDataLoaderStep(
            _countryRepository.Object,
            _holidayRepository.Object,
            _holidayFetcher.Object,
            _countryLoadStep.Object);
        var request = new CountryHolidayLoadData(It.IsAny<string>(), It.IsAny<int>());

        var act = () => loader.LoadAsync(request);

        await act.Should().ThrowAsync<FailedFetchException>();
    }

    [Test]
    public async Task LoadAsync_WhenValidHolidaysAreReturned_ShouldMapAndAddValidHolidays()
    {
        var countryCode = "ltu";
        var year = 2024;

        _countryLoadStep
            .Setup(x => x.ShouldLoadAsync(new CountryLoadData()))
            .ReturnsAsync(false);
        _holidayFetcher
            .Setup(x => x.FetchHolidaysAsync(countryCode, year))
            .ReturnsAsync([
                new FetchedCountryHoliday(new FetchedDate(11, 3, year, 1), [
                    new FetchedLocalizedName("lt", "Lietuvos nepriklausomybes atkurimo diena")
                ])
            ]);
        _countryRepository
            .Setup(x => x.GetCountryByCodeAsync(countryCode))
            .ReturnsAsync(new Country { CountryCode = countryCode, CountryName = It.IsAny<string>() });

        var loader = new CountryHolidayDataLoaderStep(
            _countryRepository.Object,
            _holidayRepository.Object,
            _holidayFetcher.Object,
            _countryLoadStep.Object);
        var request = new CountryHolidayLoadData(countryCode, year);

        await loader.LoadAsync(request);

        _holidayRepository.Verify(x => x.AddHolidaysToCountryAsync(
            It.Is<IEnumerable<Holiday>>(z =>
                z.First().Names.First().Text == "Lietuvos nepriklausomybes atkurimo diena" &&
                z.First().Dates.First().Date == new DateOnly(2024, 3, 11))),
                Times.Once);
    }
}
