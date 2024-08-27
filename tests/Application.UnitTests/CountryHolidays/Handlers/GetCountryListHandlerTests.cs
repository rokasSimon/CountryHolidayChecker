using Application.CountryHolidays.CountryList.DTO;
using Application.CountryHolidays.CountryList.Handlers;
using Application.Interfaces.Repositories;

using Domain.Entities;

using FluentAssertions;

using Moq;

namespace Application.UnitTests.CountryHolidays.Handlers;

[TestFixture]
public class GetCountryListHandlerTests
{
    private Mock<ICountryRepository> _countryRepository;

    [SetUp]
    public void Setup()
    {
        _countryRepository = new Mock<ICountryRepository>();
    }

    [Test]
    public async Task Handle_WhenRepositoryHasCountries_ShouldMapValidCountries()
    {
        _countryRepository
            .Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync([
                new Country { CountryCode = "ltu", CountryName = "Lithuania" }
            ]);

        var handler = new GetCountryListHandler(_countryRepository.Object);
        var request = new GetCountryListRequest();

        var result = await handler.Handle(request, new CancellationToken());

        result.SupportedCountries.Should().Contain(x => x.CountryCode == "ltu" && x.CountryName == "Lithuania");
    }
}
