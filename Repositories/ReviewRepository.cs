using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repositories
{

    public class ReviewRepository: IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<List<Review>> GetReviewsByMovieId(int movieId)
        {
            List<Review> reviews = await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .ToListAsync();

            return reviews;
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id) ?? null;
        }

        public async Task AddReviewAsync(Review review)
        {
            _context.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task EditReviewAsync(Review review)
        {
            _context.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveReviewAsync(int id)
        {
            var review = await GetReviewByIdAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
