using MovieApp.Models;
using System.Collections.Generic;

namespace MovieApp.Interfaces
{
    public interface ISearchStrategy
    {
        IEnumerable<Movie> Search(IEnumerable<Movie> movies, string searchString);
    }
}
