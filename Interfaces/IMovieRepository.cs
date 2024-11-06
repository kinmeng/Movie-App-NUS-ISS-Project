using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IMovieRepository
    {
        Task UpdateMoviesAsync(IEnumerable<Movie> moviesFromApi);
        Task<Movie> GetMovieByIdAsync(int movieId);
    }
}
