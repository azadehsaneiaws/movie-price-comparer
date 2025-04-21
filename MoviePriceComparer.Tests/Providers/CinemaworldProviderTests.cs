using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using MoviePriceComparer.API.Infrastructure.Providers;
using MoviePriceComparer.API.Domain.Models;
using Xunit;
using Microsoft.Extensions.Http;

public class CinemaWorldProviderTests
{
    [Fact]
    public async Task GetMoviesAsync_ShouldReturnEmptyList_WhenApiFails()
    {
        // Arrange: simulate failure
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://webjetapitest.azurewebsites.net/api/cinemaworld/")
        };
        httpClient.DefaultRequestHeaders.Add("x-access-token", "test");

        var loggerMock = new Mock<ILogger<CinemaWorldProvider>>();

        //var fakeFactory = new FakeHttpClientFactory(httpClient);
        //var provider = new CinemaWorldProvider(fakeFactory, loggerMock.Object);
        var provider = new CinemaWorldProvider(httpClient, loggerMock.Object);
        // Act
        var result = await provider.GetMoviesAsync();

        // Assert
        result.Should().BeEmpty();
    }
}


public class FakeHttpClientFactory : IHttpClientFactory
{
    private readonly HttpClient _client;

    public FakeHttpClientFactory(HttpClient client)
    {
        _client = client;
    }

    public HttpClient CreateClient(string name)
    {
        return _client;
    }
}
