using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MovieApp.Models
{

    public class Review
    {
        [Key]
        public int Id { get; set; } 
        public string Content { get; set; } 
        public int MovieId { get; set; } 
        public string? PreferredName { get; set; }
        public Movie? Movie { get; set; }

        public ApplicationUser? User { get; set; } // Navigation Property

    }
    
}
