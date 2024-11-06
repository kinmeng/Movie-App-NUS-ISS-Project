using Microsoft.EntityFrameworkCore;
using MovieApp.Controllers;
using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;
using System.Net.Http;
using System.Text.Json;

namespace MovieApp.Repositories
{
    
    public class MovieRepository: IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UpdateMoviesAsync(IEnumerable<Movie> moviesFromApi)
        {
            var apiMovieIds = moviesFromApi.Select(m => m.MovieId).ToList();
            var existingMovies = await _context.Movies
                                               .Where(m => apiMovieIds.Contains(m.MovieId))
                                               .ToListAsync();

            var newMovies = moviesFromApi.Where(apiMovie => !existingMovies.Any(dbMovie => dbMovie.MovieId == apiMovie.MovieId)).ToList();

            if (newMovies.Any())
            {
                await _context.Movies.AddRangeAsync(newMovies);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Movie> GetMovieByIdAsync(int movieId)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == movieId);
            if (movie == null)
            {
                return new Movie();
            }

            return movie;
        }
    }
}
