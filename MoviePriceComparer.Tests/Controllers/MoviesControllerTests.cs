using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MoviePriceComparer.API.Controllers;
using MoviePriceComparer.API.Domain.Interfaces;
using MoviePriceComparer.API.Domain.Models;
using Xunit;

public class MoviesControllerTests
{
    private readonly Mock<IMovieRepository> _repositoryMock;
    private readonly MoviesController _controller;

    public MoviesControllerTests()
    {
        _repositoryMock = new Mock<IMovieRepository>();
        _controller = new MoviesController(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllMovies_ShouldReturnOkWithMovies()
    {
        // Arrange
        var mockMovies = new List<Movie>
        {
            new Movie { Id = "1", Title = "Dune" }
        };
        _repositoryMock.Setup(r => r.GetAllMoviesAsync())
                       .ReturnsAsync(mockMovies);

        // Act
        var result = await _controller.GetAllMovies();

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        var movies = okResult!.Value as IEnumerable<Movie>;
        movies.Should().ContainSingle(m => m.Title == "Dune");
    }

    [Fact]
    public async Task GetCheapestMovie_ShouldReturnNotFound_WhenMovieIsNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetCheapestMovieDetailAsync("xyz")).ReturnsAsync((MovieDetail?)null);

        // Act
        var result = await _controller.GetCheapestMovie("xyz");

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetCheapestMovie_ShouldReturnOk_WhenMovieExists()
    {
        // Arrange
        var mockDetail = new MovieDetail { Id = "1", Title = "Matrix", Price = 12 };
        _repositoryMock.Setup(r => r.GetCheapestMovieDetailAsync("1"))
                       .ReturnsAsync(mockDetail);

        // Act
        var result = await _controller.GetCheapestMovie("1");

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
    }
}

