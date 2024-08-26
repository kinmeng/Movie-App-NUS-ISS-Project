using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Manager;
using MovieApp.Models;
using System.Diagnostics;

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

        public IActionResult About()
        {
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
