using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Interfaces;
using MovieApp.Manager;
using MovieApp.Models;
using MovieApp.Services;
using NuGet.Protocol.Core.Types;

namespace MovieApp.Controllers
{
    public class MoviesController : Controller
    {

        private readonly MovieService _movieService;

        private readonly ApplicationUserManager _userManager;

        public MoviesController(MovieService movieService, ApplicationUserManager userManager)
        {
            _userManager = userManager;
            _movieService = movieService;
        }
        //[Route("/Movies")]
        public async Task<IActionResult> Index(string searchString, string sortOrder, string searchType)
        {
            var user = HttpContext.User;
            var preferredName = await _userManager.GetPreferredNameAsync(user);
            ViewData["PreferredName"] = preferredName;

            await _movieService.UpdateMoviesAsync();
            var movies = await _movieService.GetMoviesAsync();

            //// Apply search filter
            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    movies = movies.Where(m => m.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            //}

            // Determine search strategy
            ISearchStrategy searchStrategy = searchType switch
            {
                "ReleaseDate" => new ReleaseDateSearchStrategy(),
                _ => new TitleSearchStrategy(), // Default to title search
            };

            // Apply search strategy
            movies = await _movieService.SearchMoviesAsync(searchStrategy, searchString);


            // Apply sorting based on the sortOrder parameter
            movies = sortOrder switch
            {
                "title_desc" => movies.OrderByDescending(m => m.Title),
                "release_asc" => movies.OrderBy(m => m.ReleaseDate),
                "release_desc" => movies.OrderByDescending(m => m.ReleaseDate),
                "rating_asc" => movies.OrderBy(m => m.Rating),
                "rating_desc" => movies.OrderByDescending(m => m.Rating),
                _ => movies.OrderBy(m => m.Title), // Default sort by title ascending
            };

            // Pass searchString and sortOrder to the view
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentSearchType"] = searchType;

            return View(movies.ToList());
        }

    }
}