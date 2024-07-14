using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Manager;
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
        public async Task<IActionResult> Index()
        {
            var user = HttpContext.User;
            var preferredName = await _userManager.GetPreferredNameAsync(user);
            ViewData["PreferredName"] = preferredName;

            await _movieService.UpdateMoviesAsync();
            var movies = await _movieService.GetMoviesAsync();
            return View(movies);
        }
        //public string Index()
        //{
        //    return "This is my default action...";
        //}
    }
}
