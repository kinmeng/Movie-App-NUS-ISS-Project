using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    public class MoviesController : Controller
    {

        private readonly MovieService _movieService;

        public MoviesController(MovieService movieService)
        {
            _movieService = movieService;
        }
        //[Route("/Movies")]
        public async Task<IActionResult> Index()
        {
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
