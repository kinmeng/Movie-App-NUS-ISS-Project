using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Manager;
using MovieApp.Models;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace MovieApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationUserManager _userManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = HttpContext.User;
            var preferredName = await _userManager.GetPreferredNameAsync(user);

            ViewData["PreferredName"] = preferredName;

            // Log message to verify method execution
            _logger.LogInformation($"Index method called. PreferredName: {preferredName}");

            return View();
        }

        public async Task<IActionResult> About()
        {
            var user = HttpContext.User;
            var preferredName = await _userManager.GetPreferredNameAsync(user);
            ViewData["PreferredName"] = preferredName;

            return View();
        }

        [HttpPost]
        public IActionResult About(string name, string email, string message)
        {
            // Here you would typically process the feedback, e.g., save it to a database or send an email.

            //Log feedback submission
            _logger.LogInformation($"Feedback submitted by {name} ({email}): {message}");

            // Indicate that the feedback was successfully sent.
            ViewBag.FeedbackSent = true;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
