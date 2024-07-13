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
        public string? UserId { get; set; }
        public Movie? Movie { get; set; }

        public IdentityUser? User { get; set; } // Navigation Property

    }
    
}
