using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using MoviePriceComparer.API.Domain.Interfaces;
using MoviePriceComparer.API.Domain.Models;

public class MovieRepositoryTests
{
    private readonly Mock<IMemoryCache> _cacheMock = new();
    private readonly Mock<ILogger<MovieRepository>> _loggerMock = new();
    private readonly List<Mock<IMovieProvider>> _providerMocks = new();

    private MovieRepository CreateRepositoryWithProviders(params Movie[] movies)
    {
        var providerMock = new Mock<IMovieProvider>();
        providerMock.Setup(p => p.GetMoviesAsync()).ReturnsAsync(movies);
        _providerMocks.Add(providerMock);

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        return new MovieRepository(new[] { providerMock.Object }, memoryCache, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllMoviesAsync_ShouldReturnAllMoviesWithoutDuplicates()
    {
        // Arrange
        var movie = new Movie { Id = "1", Title = "Inception", Year = 2010 };
        var repo = CreateRepositoryWithProviders(movie, movie); // duplicate

        // Act
        var result = await repo.GetAllMoviesAsync();

        // Assert
        result.Should().HaveCount(1);
        result.Should().ContainSingle(m => m.Title == "Inception");
    }

    [Fact]
    public async Task GetCheapestMovieDetailAsync_ShouldReturnCheapestPrice()
    {
        // Arrange
        var detail1 = new MovieDetail { Id = "1", Title = "Dune", Price = 20, Provider = "CW" };
        var detail2 = new MovieDetail { Id = "1", Title = "Dune", Price = 15, Provider = "FW" };

        var provider1 = new Mock<IMovieProvider>();
        provider1.Setup(p => p.GetMovieDetailsAsync("1")).ReturnsAsync(detail1);

        var provider2 = new Mock<IMovieProvider>();
        provider2.Setup(p => p.GetMovieDetailsAsync("1")).ReturnsAsync(detail2);

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var repo = new MovieRepository(new[] { provider1.Object, provider2.Object }, memoryCache, _loggerMock.Object);

        // Act
        var result = await repo.GetCheapestMovieDetailAsync("1");

        // Assert
        result.Should().NotBeNull();
        result!.Price.Should().Be(15);
        result.Provider.Should().Be("FW");
    }
}
