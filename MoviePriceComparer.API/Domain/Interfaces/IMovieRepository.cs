using MoviePriceComparer.API.Domain.Models;

    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<MovieDetail?> GetCheapestMovieDetailAsync(string movieId);
    }


