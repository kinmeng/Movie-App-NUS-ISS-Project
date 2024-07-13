using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Services;
using MovieApp.Models;
using System.Security.Claims;
using static MovieApp.Models.Review;
using Microsoft.AspNetCore.Identity;

namespace MovieApp.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly MovieService _movieService;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewController(ApplicationDbContext context, MovieService movieService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _movieService = movieService;
            _userManager = userManager;
        }

        // GET: Review/ViewReviews/5
        public async Task<IActionResult> ViewReviews(int movieId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .ToListAsync();

            return View(reviews);
        }

        // GET: Review/Create/5
        public IActionResult Create(int movieId)
        {
            ViewBag.MovieId = movieId;
            return View();
        }

        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            review.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();
            review.User = await _userManager.GetUserAsync(HttpContext.User);

            // Retrieve the movie
            var movie = await _movieService.GetMovieByIdAsync(review.MovieId);
            if (movie == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid MovieId.");
            }
            else
            {
                review.Movie = movie;
            }

            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Movies");
            }

            // Debug: Inspect ModelState errors
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                // Log or inspect each validation error
                Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
            }
            return View(review);
        }

        // Other actions: Edit, Delete, etc.
    }
}
