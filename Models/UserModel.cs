using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace MovieApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Preferred Name is required")]
        public string PreferredName { get; set; }
    }
}
