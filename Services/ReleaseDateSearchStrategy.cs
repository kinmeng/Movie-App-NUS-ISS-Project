using MovieApp.Models;
using MovieApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieApp.Services
{
    public class ReleaseDateSearchStrategy : ISearchStrategy
    {
        public IEnumerable<Movie> Search(IEnumerable<Movie> movies, string searchString)
        {
            // Try to parse the search string as a full date
            if (DateTime.TryParse(searchString, out DateTime searchDate))
            {
                // If a full date is provided, return movies released on that date
                return movies.Where(m => m.ReleaseDate.Date == searchDate.Date);
            }

            // Try to parse the search string as a year
            if (int.TryParse(searchString, out int searchYear))
            {
                // If only a year is provided, return movies released in that year
                return movies.Where(m => m.ReleaseDate.Month == searchYear);
            }

            // If the search string cannot be parsed as a date or year, return all movies (or handle as needed)
            return movies;
        }
    }
}
