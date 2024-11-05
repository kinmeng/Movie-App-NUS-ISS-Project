using Microsoft.Extensions.Options;
using MovieApp.Controllers;
using System.Text.Json;
using MovieApp.Models;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Repositories;

namespace MovieApp.Services
{
    public class MovieService: IMovieService
    {

        private readonly HttpClient _httpClient;
        private readonly TmdbSettings _settings;
        private readonly MovieRepository _repository;
        public MovieService(HttpClient httpClient, IOptions<TmdbSettings> settings, MovieRepository movieRepository)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _repository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            var response = await _httpClient.GetAsync($"https://api.themoviedb.org/3/movie/popular?api_key={_settings.ApiKey}&language=en-US&page=1");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var movieResponse = JsonSerializer.Deserialize<MovieResponse>(responseBody);
            return movieResponse?.results ?? [];
        }

        public async Task UpdateMoviesAsync()
        {
            var moviesFromApi = await GetMoviesAsync();

            await _repository.UpdateMoviesAsync(moviesFromApi);
        }

        public async Task<Movie> GetMovieByIdAsync(int movieId)
        {
            return await _repository.GetMovieByIdAsync(movieId);
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
