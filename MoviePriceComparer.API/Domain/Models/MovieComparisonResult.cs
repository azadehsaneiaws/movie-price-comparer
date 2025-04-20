namespace MoviePriceComparer.API.Domain.Models
{
    public class MovieComparisonResult
    {
        public MovieDetail MovieDetails { get; set; }
        public Dictionary<string, decimal?> ProviderPrices { get; set; } = new Dictionary<string, decimal?>();
        public string CheapestProvider { get; set; }
        public decimal? CheapestPrice { get; set; }
    }
}
