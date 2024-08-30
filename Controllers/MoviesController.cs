using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Manager;
using MovieApp.Models;
using MovieApp.Services;

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
        public async Task<IActionResult> Index(string sortOrder)
        {
            var user = HttpContext.User;
            var preferredName = await _userManager.GetPreferredNameAsync(user);
            ViewData["PreferredName"] = preferredName;

            await _movieService.UpdateMoviesAsync();
            var movies = await _movieService.GetMoviesAsync();

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

            return View(movies.ToList());
        }
        //public string Index()
        //{
        //    return "This is my default action...";
        //
    }
}