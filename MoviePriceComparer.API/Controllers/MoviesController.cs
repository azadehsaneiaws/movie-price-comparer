using Microsoft.AspNetCore.Mvc;
using MoviePriceComparer.API.Domain.Interfaces;
using MoviePriceComparer.API.Domain.Models;

namespace MoviePriceComparer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            return await _movieRepository.GetAllMoviesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetail>> GetCheapestMovie(string id)
        {
            var detail = await _movieRepository.GetCheapestMovieDetailAsync(id);
            if (detail == null) return NotFound();
            return detail;
        }
    }
}
