using MovieApp.Models;

namespace MovieApp.Models
{
    public class MovieReviewsViewModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
