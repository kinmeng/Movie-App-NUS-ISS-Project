﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data;
using MovieApp.Services;
using MovieApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MovieApp.Manager;
namespace MovieApp.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly MovieService _movieService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationUserManager _applicationUserManager;
        public ReviewController(ApplicationDbContext context, MovieService movieService, UserManager<ApplicationUser> userManager, ApplicationUserManager applicationUserManager)
        {
            _context = context;
            _movieService = movieService;
            _userManager = userManager;
            _applicationUserManager = applicationUserManager;
        }

        // GET: Review/ViewReviews/5
        public async Task<IActionResult> ViewReviews(int movieId)
        {

            var user = HttpContext.User;
            var preferredName = await _applicationUserManager.GetPreferredNameAsync(user);
            ViewData["PreferredName"] = preferredName;

            var movie = await _movieService.GetMovieByIdAsync(movieId);
            var reviews = await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .ToListAsync();

            var viewModel = new MovieReviewsViewModel
            {
                Movie = movie,
                Reviews = reviews
            };

            return View(viewModel);
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
            var user = HttpContext.User;
            review.PreferredName = await _applicationUserManager.GetPreferredNameAsync(user);
            review.User = await _userManager.GetUserAsync(user);

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
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_submitReview", review); // Return partial view for AJAX
                }
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

        // GET: Review/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var user = HttpContext.User;
            var preferredName = await _applicationUserManager.GetPreferredNameAsync(user);

            ViewData["PreferredName"] = preferredName;
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Review/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,MovieId,PreferredName")] Review review)
        {

            var user = HttpContext.User;
            var preferredName = await _applicationUserManager.GetPreferredNameAsync(user);

            ViewData["PreferredName"] = preferredName;
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ViewReviews", new { movieId = review.MovieId });
            }
            return View(review);
        }

        // GET: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewReviews", new { movieId = review.MovieId });
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
