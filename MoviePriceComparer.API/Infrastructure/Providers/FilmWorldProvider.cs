using MoviePriceComparer.API.Domain.Interfaces;
using MoviePriceComparer.API.Domain.Models;
using System.Text.Json;

namespace MoviePriceComparer.API.Infrastructure.Providers

{   //S: Each provider is responsible only for talking to its corresponding external API.
    //O: Each provider class (CinemaWorldProvider, FilmWorldProvider) implements the shared interface IMovieProvider.
    public class FilmWorldProvider : IMovieProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FilmWorldProvider> _logger;

        public string ProviderName => "FilmWorld";

        public FilmWorldProvider(IHttpClientFactory httpClientFactory, ILogger<FilmWorldProvider> logger)
        {
            _httpClient = httpClientFactory.CreateClient(ProviderName);
            _logger = logger;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("movies");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var moviesResponse = JsonSerializer.Deserialize<MoviesResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return moviesResponse?.Movies ?? new List<Movie>();
                }

                _logger.LogWarning($"Failed to get movies from {ProviderName}. Status code: {response.StatusCode}");
                return new List<Movie>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting movies from {ProviderName}");
                return new List<Movie>();
            }
        }

        public async Task<MovieDetail> GetMovieDetailsAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"movie/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<MovieDetail>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }

                _logger.LogWarning($"Failed to get movie details from {ProviderName} for ID {id}. Status code: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting movie details from {ProviderName} for ID {id}");
                return null;
            }
        }
    }

    // MoviesResponse class used for deserialization
    internal class MoviesResponse
    {
        public List<Movie> Movies { get; set; }
    }
}
}
