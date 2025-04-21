using MoviePriceComparer.API.Domain.Models;

namespace MoviePriceComparer.API.Domain.Interfaces
{
    public interface IMovieProvider
    {
        string ProviderName { get; }
        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task<MovieDetail?> GetMovieDetailsAsync(string id);
    }

}
