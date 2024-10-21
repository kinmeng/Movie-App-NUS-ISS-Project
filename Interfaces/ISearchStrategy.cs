using MovieApp.Models;
using System.Collections.Generic;

namespace MovieApp.Interfaces
{
    public abstract class BaseSearchStrategy : ISearchStrategy
    {
        public IEnumerable<Movie> Search(IEnumerable<Movie> movies, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return movies;

            return PerformSearch(movies, searchString);
        }

        protected abstract IEnumerable<Movie> PerformSearch(IEnumerable<Movie> movies, string searchString);
    }

    public interface ISearchStrategy
    {
        IEnumerable<Movie> Search(IEnumerable<Movie> movies, string searchString);
    }
}
