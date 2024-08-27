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
public class CountryDataLoaderStepTests
{
    private Mock<ICountryDataFetcher> _countryFetcher;
    private Mock<ICountryRepository> _countryRepository;

    [SetUp]
    public void Setup()
    {
        _countryFetcher = new Mock<ICountryDataFetcher>();
        _countryRepository = new Mock<ICountryRepository>();
    }

    [Test]
    public async Task ShouldLoadAsync_WhenRepositoryIsEmpty_ShouldReturnTrue()
    {
        _countryRepository
            .Setup(x => x.IsEmptyAsync())
            .ReturnsAsync(true);

        var loader = new CountryDataLoaderStep(_countryRepository.Object, _countryFetcher.Object);
        var request = new CountryLoadData();

        var result = await loader.ShouldLoadAsync(request);

        result.Should().Be(true);
    }

    [Test]
    public async Task ShouldLoadAsync_WhenRepositoryIsNotEmpty_ShouldReturnFalse()
    {
        _countryRepository
            .Setup(x => x.IsEmptyAsync())
            .ReturnsAsync(false);

        var loader = new CountryDataLoaderStep(_countryRepository.Object, _countryFetcher.Object);
        var request = new CountryLoadData();

        var result = await loader.ShouldLoadAsync(request);

        result.Should().Be(false);
    }

    [Test]
    public async Task LoadAsync_WhenHolidayServiceIsAvailable_ShouldMapAndAddCountries()
    {
        var fetchedCountry = new FetchedCountry("ltu", "Lithuania");
        var expectedCountry = new Country { CountryCode = "ltu", CountryName = "Lithuania" };

        _countryFetcher
            .Setup(x => x.FetchCountriesAsync())
            .ReturnsAsync([ fetchedCountry ]);

        var loader = new CountryDataLoaderStep(_countryRepository.Object, _countryFetcher.Object);
        var request = new CountryLoadData();

        await loader.LoadAsync(request);

        _countryRepository.Verify(x => x.AddCountriesAsync(
            It.Is<IEnumerable<Country>>(x => x.First().CountryCode == expectedCountry.CountryCode)),
            Times.Once);
    }

    [Test]
    public async Task LoadAsync_WhenHolidayServiceIsNotAvailable_ShouldThrowFailedFetchException()
    {
        _countryFetcher
            .Setup(x => x.FetchCountriesAsync())
            .ThrowsAsync(new FailedFetchException(It.IsAny<string>()));

        var loader = new CountryDataLoaderStep(_countryRepository.Object, _countryFetcher.Object);
        var request = new CountryLoadData();

        var act = () => loader.LoadAsync(request);

        await act.Should().ThrowAsync<FailedFetchException>();
    }
}
