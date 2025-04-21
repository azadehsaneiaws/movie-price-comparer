
    using global::MoviePriceComparer.API.Domain.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using MoviePriceComparer.API.Domain.Interfaces;
    using MoviePriceComparer.API.Domain.Models;

    namespace MoviePriceComparer.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class MoviesController : ControllerBase
        {
            private readonly IMovieRepository _repository;

            public MoviesController(IMovieRepository repository)
            {
                _repository = repository;
            }

            // GET api/movies
            [HttpGet]
            public async Task<IActionResult> GetAllMovies()
            {
                var movies = await _repository.GetAllMoviesAsync();
                return Ok(movies);
            }

            // GET api/movies/{id}/cheapest
            [HttpGet("{id}/cheapest")]
            public async Task<IActionResult> GetCheapestMovie(string id)
            {
                var movie = await _repository.GetCheapestMovieDetailAsync(id);

                if (movie == null)
                    return NotFound($"No movie details found for ID: {id}");

                return Ok(movie);
            }
        }
    }


