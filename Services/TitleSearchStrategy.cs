using MovieApp.Models;
using MovieApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieApp.Services
{
    public class TitleSearchStrategy : BaseSearchStrategy
    {
        protected override IEnumerable<Movie> PerformSearch(IEnumerable<Movie> movies, string searchString)
        {
            return movies.Where(m => m.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
        }
    }
}