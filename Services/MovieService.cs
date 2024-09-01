using Microsoft.Extensions.Options;
using MovieApp.Controllers;
using System.Text.Json;
using MovieApp.Models;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Interfaces;

namespace MovieApp.Services
{
    public class MovieService
    {

        private readonly HttpClient _httpClient;
        private readonly TmdbSettings _settings;
        private readonly ApplicationDbContext _context;
        public MovieService(HttpClient httpClient, IOptions<TmdbSettings> settings, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/popular?api_key={_settings.ApiKey}&language=en-US&page=1");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var movieResponse = JsonSerializer.Deserialize<MovieResponse>(responseBody);
            return movieResponse.results;
        }

        public async Task UpdateMoviesAsync()
        {
            var moviesFromApi = await GetMoviesAsync();

            var apiMovieIds = moviesFromApi.Select(m => m.MovieId).ToList();
            var existingMovies = await _context.Movies
                                               .Where(m => apiMovieIds.Contains(m.MovieId))
                                               .ToListAsync();

            var newMovies = moviesFromApi.Where(apiMovie => !existingMovies.Any(dbMovie => dbMovie.MovieId == apiMovie.MovieId)).ToList();


            //foreach (var movie in newMovies)
            //{
            //    // Ensure the Id is not set explicitly
            //    movie.Id = 0;
            //}

            if (newMovies.Any())
            {
                await _context.Movies.AddRangeAsync(newMovies);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Movie> GetMovieByIdAsync(int movieId)
        {
            
            var movie= await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == movieId);
            if (movie == null)
            {
                return null;
            }

            else
            {
                return movie;
            }
        }

        public async Task<IEnumerable<Movie>> SearchMoviesAsync(ISearchStrategy searchStrategy, string searchString)
        {
            var movies = await GetMoviesAsync();
            if (string.IsNullOrEmpty(searchString))
            {
                return movies;
            }
            return searchStrategy.Search(movies, searchString);
        }
    }
}
