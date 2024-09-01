using MovieApp.Models;
using MovieApp.Interfaces; // Ensure this matches where you placed ISearchStrategy
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieApp.Services
{
    public class TitleSearchStrategy : ISearchStrategy
    {
        public IEnumerable<Movie> Search(IEnumerable<Movie> movies, string searchString)
        {
            return movies.Where(m => m.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
        }
    }
}
