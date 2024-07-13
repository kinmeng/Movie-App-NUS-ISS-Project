using System.Text.Json.Serialization;
using static MovieApp.Models.Review;

namespace MovieApp.Models
{
    //public class MovieModel
    //{
        public class MovieResponse{
        
            public List<Movie>results { get; set; }
        }
        public class Movie
        {
        //[JsonPropertyName("id")]
        //public int Id { get; set; }
            public int Id { get; set; } // Identity column

            [JsonPropertyName("title")]
            public string? Title { get; set; }

            [JsonPropertyName("genre_ids")]
            public List<int>? GenreIds { get; set; }

            [JsonPropertyName("release_date")]
            public DateTime ReleaseDate { get; set; }

            [JsonPropertyName("vote_average")]
            public double Rating { get; set; }

            [JsonPropertyName("id")]
            public int MovieId { get; set; }

            public ICollection<Review> Reviews { get; set; }
    }
    //}

}
