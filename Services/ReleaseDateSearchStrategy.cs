using MovieApp.Models;
using MovieApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieApp.Services
{
    public class ReleaseDateSearchStrategy : BaseSearchStrategy
    {
        protected override IEnumerable<Movie> PerformSearch(IEnumerable<Movie> movies, string searchString)
        {
            if (DateTime.TryParse(searchString, out DateTime searchDate))
            {
                return movies.Where(m => m.ReleaseDate.Date == searchDate.Date);
            }

            if (int.TryParse(searchString, out int searchYear))
            {
                return movies.Where(m => m.ReleaseDate.Year == searchYear);
            }

            return movies;
        }
    }
}