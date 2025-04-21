using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MoviePriceComparer.API.Domain.Interfaces;
using MoviePriceComparer.API.Domain.Models;
using Polly;
using Polly.Retry;


public class MovieRepository : IMovieRepository
        {
            private readonly IEnumerable<IMovieProvider> _providers;
            private readonly IMemoryCache _cache;
            private readonly ILogger<MovieRepository> _logger;
            private readonly AsyncRetryPolicy _retryPolicy;

            public MovieRepository(IEnumerable<IMovieProvider> providers, IMemoryCache cache, ILogger<MovieRepository> logger)
            {
                _providers = providers;
                _cache = cache;
                _logger = logger;

                // Retry policy: try 3 times with 500ms delay
                _retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(500),
                        (exception, timeSpan, retryCount, context) =>
                        {
                            _logger.LogWarning(exception, "Retry {RetryCount} failed after {Delay}", retryCount, timeSpan);
                        });
            }

            public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
            {
                // Try cache first
                if (_cache.TryGetValue("AllMovies", out List<Movie> cached))
                {
                    return cached;
                }

                var tasks = _providers.Select(provider =>
                    _retryPolicy.ExecuteAsync(() => provider.GetMoviesAsync())
                );

                var results = await Task.WhenAll(tasks);

                var allMovies = results.SelectMany(m => m).ToList();

                // Deduplicate (by Title + Year)
                var uniqueMovies = allMovies
                    .GroupBy(m => new { m.Title, m.Year })
                    .Select(g => g.First())
                    .ToList();

                // Cache for 10 mins
                _cache.Set("AllMovies", uniqueMovies, TimeSpan.FromMinutes(10));

                return uniqueMovies;
            }

            public async Task<MovieDetail?> GetCheapestMovieDetailAsync(string movieId)
            {
                var detailTasks = _providers.Select(provider =>
                    _retryPolicy.ExecuteAsync(() => provider.GetMovieDetailsAsync(movieId))
                );

                var results = await Task.WhenAll(detailTasks);

                var validResults = results.Where(d => d != null).ToList();

                if (!validResults.Any())
                    return null;

                var cheapest = validResults.OrderBy(d => d!.Price).FirstOrDefault();

                return cheapest;
            }
        }
