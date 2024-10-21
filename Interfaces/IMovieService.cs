using MovieApp.Models;
using MovieApp.Interfaces;

namespace MovieApp.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetMoviesAsync();
        Task UpdateMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int movieId);
        Task<IEnumerable<Movie>> SearchMoviesAsync(ISearchStrategy searchStrategy, string searchString);
    }
}