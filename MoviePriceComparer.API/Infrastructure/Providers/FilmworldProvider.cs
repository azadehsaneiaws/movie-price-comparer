namespace MoviePriceComparer.API.Infrastructure.Providers
{
    using System.Net.Http;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using MoviePriceComparer.API.Domain.Models;
    using MoviePriceComparer.API.Domain.Interfaces;


    public class FilmworldProvider : IMovieProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FilmworldProvider> _logger;

        public string ProviderName => "Filmworld";

        public FilmworldProvider(HttpClient httpClient, ILogger<FilmworldProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("movies");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("FilmWorld GetMovies failed. Status: {StatusCode}", response.StatusCode);
                    return Enumerable.Empty<Movie>();
                }

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Received from filmaworld: {Content}", content); // <== Add this

                var root = JsonSerializer.Deserialize<MovieListResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return root?.Movies ?? Enumerable.Empty<Movie>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetMoviesAsync");
                return Enumerable.Empty<Movie>();
            }
        }


        public async Task<MovieDetail?> GetMovieDetailsAsync(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"movie/{id}");
                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();
                var detail = JsonSerializer.Deserialize<MovieDetail>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (detail != null)
                    detail.Provider = ProviderName;

                return detail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching details for {Id}", id);
                return null;
            }
        }
    }
}

