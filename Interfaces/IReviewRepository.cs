using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetReviewsByMovieId(int movieId);
        Task<Review?> GetReviewByIdAsync(int id);
        Task AddReviewAsync(Review review);
        Task EditReviewAsync(Review review);
        Task RemoveReviewAsync(int id);
        bool ReviewExists(int id);
    }
}
