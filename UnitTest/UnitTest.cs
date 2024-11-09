using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieApp.Controllers;
using MovieApp.Interfaces;
using MovieApp.Manager;
using MovieApp.Models;
using System.Security.Claims;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTests
{
    [TestClass]
    public class MoviesControllerTests
    {
        private Mock<IMovieService> _mockMovieService;
        private ApplicationUserManager _userManager;
        private List<Movie> _testMovies;
        private MoviesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockMovieService = new Mock<IMovieService>();
            var store = new Mock<IUserStore<ApplicationUser>>();
            var optionsAccessor = Options.Create(new IdentityOptions());
            var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            var userValidators = new List<IUserValidator<ApplicationUser>>();
            var passwordValidators = new List<IPasswordValidator<ApplicationUser>>();
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new IdentityErrorDescriber();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<ApplicationUser>>>();

            _userManager = new ApplicationUserManager(
                store.Object, optionsAccessor, passwordHasher.Object, userValidators, passwordValidators,
                keyNormalizer.Object, errors, services.Object, logger.Object);
            _testMovies = new List<Movie>
            {
                new Movie { MovieId = 1, Title = "Old Classic", ReleaseDate = DateTime.Parse("1980-01-01"), Rating = 8.5 },
                new Movie { MovieId = 2, Title = "New Blockbuster", ReleaseDate = DateTime.Parse("2023-06-15"), Rating = 7.8 },
                new Movie { MovieId = 3, Title = "Indie Gem", ReleaseDate = DateTime.Parse("2022-03-10"), Rating = 8.2 },
                new Movie { MovieId = 4, Title = "Sci-Fi Epic", ReleaseDate = DateTime.Parse("2021-11-05"), Rating = 7.9 },
                new Movie { MovieId = 5, Title = "Timeless Masterpiece", ReleaseDate = DateTime.Parse("1995-09-22"), Rating = 9.0 }
            };


            _mockMovieService.Setup(service => service.UpdateMoviesAsync()).Returns(Task.CompletedTask);
            _mockMovieService.Setup(service => service.GetMoviesAsync()).ReturnsAsync(_testMovies);
            _mockMovieService.Setup(service => service.SearchMoviesAsync(It.IsAny<ISearchStrategy>(), It.IsAny<string>()))
                .ReturnsAsync(_testMovies);

            _controller = new MoviesController(_mockMovieService.Object, _userManager);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser@example.com"),
            }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TestMethod]
        public async Task Index_ReturnsViewResult_WithListOfMovies()
        {
            // Act
            var result = await _controller.Index(null, null, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsInstanceOfType(viewResult.Model, typeof(List<Movie>));
            var model = viewResult.Model as List<Movie>;
            Assert.AreEqual(5, model.Count);
        }

        [TestMethod]
        public async Task Index_SortsByTitleDescending_WhenSortOrderIsTitleDesc()
        {
            // Act
            var result = await _controller.Index(null, "title_desc", null);
            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Movie>;
            Assert.AreEqual(5, model.Count);
            Assert.AreEqual("Timeless Masterpiece", model[0].Title);
            Assert.AreEqual("Sci-Fi Epic", model[1].Title);
            Assert.AreEqual("Old Classic", model[2].Title);
            Assert.AreEqual("New Blockbuster", model[3].Title);
            Assert.AreEqual("Indie Gem", model[4].Title);
        }
        [TestMethod]
        public async Task Index_SortsByReleaseDateAscending_WhenSortOrderIsReleaseAsc()
        {
            // Act
            var result = await _controller.Index(null, "release_asc", null);

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Movie>;
            Assert.AreEqual(5, model.Count);
            Assert.AreEqual("Old Classic", model[0].Title);
            Assert.AreEqual("Timeless Masterpiece", model[1].Title);
            Assert.AreEqual("Sci-Fi Epic", model[2].Title);
            Assert.AreEqual("Indie Gem", model[3].Title);
            Assert.AreEqual("New Blockbuster", model[4].Title);
        }

        [TestMethod]
        public async Task Index_SortsByReleaseDateDescending_WhenSortOrderIsReleaseDesc()
        {
            // Act
            var result = await _controller.Index(null, "release_desc", null);

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Movie>;
            Assert.AreEqual(5, model.Count);
            Assert.AreEqual("New Blockbuster", model[0].Title);
            Assert.AreEqual("Indie Gem", model[1].Title);
            Assert.AreEqual("Sci-Fi Epic", model[2].Title);
            Assert.AreEqual("Timeless Masterpiece", model[3].Title);
            Assert.AreEqual("Old Classic", model[4].Title);
        }


        [TestMethod]
        public async Task Index_SearchesByTitle_WhenSearchStringProvided()
        {
            // Arrange
            _mockMovieService.Setup(service => service.SearchMoviesAsync(It.IsAny<ISearchStrategy>(), "Indie Gem"))
                .ReturnsAsync(_testMovies.Where(m => m.Title.Contains("Indie Gem")).ToList());

            // Act
            var result = await _controller.Index("Indie Gem", null, null);

            // Assert
            var viewResult = result as ViewResult;
            var model = viewResult.Model as List<Movie>;
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Indie Gem", model[0].Title);
        }

        [TestMethod]
        public async Task Index_SetsViewData_WithCorrectValues()
        {
            // Act
            var result = await _controller.Index("searchString", "sortOrder", "searchType");

            // Assert
            var viewResult = result as ViewResult;
            Assert.AreEqual("searchString", viewResult.ViewData["CurrentFilter"]);
            Assert.AreEqual("sortOrder", viewResult.ViewData["CurrentSort"]);
            Assert.AreEqual("searchType", viewResult.ViewData["CurrentSearchType"]);
        }
    }
}