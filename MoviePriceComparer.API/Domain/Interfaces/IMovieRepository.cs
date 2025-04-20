using MoviePriceComparer.API.Domain.Models;

namespace MoviePriceComparer.API.Domain.Interfaces
{
    public interface IMovieRepository
    {
            Task<IEnumerable<Movie>> GetAllMoviesAsync();
            Task<MovieComparisonResult> GetMovieComparisonAsync(string id);

    }
}
