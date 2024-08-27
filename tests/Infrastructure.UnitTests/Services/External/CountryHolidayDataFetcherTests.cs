using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Common.Exceptions;
using Infrastructure.Configurations;
using Infrastructure.Services;
using Microsoft.Extensions.Options;

using Moq.Protected;

using Moq;
using FluentAssertions;

namespace Infrastructure.UnitTests.Services.External;

public class CountryHolidayDataFetcherTests
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
    public async Task FetchHolidaysAsync_WhenServiceIsAvailable_ShouldReturnParsedHolidays()
    {
        var jsonResponse =
        """
        [
          {
            "date": {
              "day": 1,
              "month": 1,
              "year": 2024,
              "dayOfWeek": 1
            },
            "name": [
              {
                "lang": "ja",
                "text": "元日 Ganjitsu"
              },
              {
                "lang": "en",
                "text": "New Year's Day"
              }
            ],
            "holidayType": "public_holiday"
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
            .Returns(new CountryHolidayFetcherOptions { BaseUrl = "", CountryListUrl = "", CountryHolidayUrl = "https://base.com/" });
        _httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(client);
        var fetcher = new CountryHolidayDataFetcher(_httpClientFactory.Object, _options.Object);

        var result = await fetcher.FetchHolidaysAsync(It.IsAny<string>(), It.IsAny<int>());

        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task FetchHolidaysAsync_WhenServiceIsNotAvailable_ShouldThrowFailedFetch()
    {
        var clientMock = new Mock<DelegatingHandler>();
        clientMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .Throws<HttpRequestException>()
            .Verifiable();
        var client = new HttpClient(clientMock.Object);

        _options
            .Setup(x => x.Value)
            .Returns(new CountryHolidayFetcherOptions { BaseUrl = "", CountryListUrl = "", CountryHolidayUrl = "https://base.com/" });
        _httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(client);
        var fetcher = new CountryHolidayDataFetcher(_httpClientFactory.Object, _options.Object);

        var act = () => fetcher.FetchHolidaysAsync(It.IsAny<string>(), It.IsAny<int>());

        await act.Should().ThrowAsync<FailedFetchException>();
    }
}
