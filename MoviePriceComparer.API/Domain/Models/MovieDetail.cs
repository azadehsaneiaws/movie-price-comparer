namespace MoviePriceComparer.API.Domain.Models
{
    public class MovieDetail : Movie
    {
        public string Poster { get; set; }
        public string Plot { get; set; }
        public decimal Price { get; set; }
        public string Provider { get; set; }
    }
}
