using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Infrastructure.Common.Exceptions;
using Infrastructure.Configurations;
using Infrastructure.Services;

using Microsoft.Extensions.Options;

using Moq;
using Moq.Protected;

namespace Infrastructure.UnitTests.Services.External;

[TestFixture]
public class CountryDataFetcherTests
{
    private Mock<IOptions<CountryHolidayFetcherOptions>> _options;
    private Mock<IHttpClientFactory> _httpClientFactory;

    [SetUp]
    public void Setup()
    {
        _options = new Mock<IOptions<CountryHolidayFetcherOptions>>();
        _httpClientFactory = new Mock<IHttpClientFactory>();
    }

    [Test]
    public async Task FetchCountriesAsync_WhenServiceIsAvailable_ShouldReturnParsedCountries()
    {
        var jsonResponse =
        """
        [
          {
            "countryCode": "ago",
            "fullName": "Angola"
          }
        ]
        """;
        var clientMock = new Mock<DelegatingHandler>();
        clientMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(jsonResponse) })
            .Verifiable();

        var client = new HttpClient(clientMock.Object);

        _options
            .Setup(x => x.Value)
            .Returns(new CountryHolidayFetcherOptions { BaseUrl = "", CountryListUrl = "https://base.com/", CountryHolidayUrl = "" });
        _httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(client);
        var fetcher = new CountryDataFetcher(_httpClientFactory.Object, _options.Object);

        var result = await fetcher.FetchCountriesAsync();

        result.Should().Contain(x => x.CountryCode == "ago" && x.FullName == "Angola");
    }

    [Test]
    public async Task FetchCountriesAsync_WhenServiceIsNotAvailable_ShouldThrowFailedFetch()
    {
        var clientMock = new Mock<DelegatingHandler>();
        clientMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Throws<HttpRequestException>()
            .Verifiable();

        var client = new HttpClient(clientMock.Object);

        _options
            .Setup(x => x.Value)
            .Returns(new CountryHolidayFetcherOptions { BaseUrl = "", CountryListUrl = "https://base.com/", CountryHolidayUrl = "" });
        _httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(client);
        var fetcher = new CountryDataFetcher(_httpClientFactory.Object, _options.Object);

        var act = fetcher.FetchCountriesAsync;

        await act.Should().ThrowAsync<FailedFetchException>();
    }
}
