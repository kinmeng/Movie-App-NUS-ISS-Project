using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieApp.Models;

namespace MovieApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Reviews)         // A movie can have many reviews
                .WithOne(r => r.Movie)           // Each review belongs to one movie
                .HasForeignKey(r => r.MovieId)
                .HasPrincipalKey(m => m.MovieId); // Foreign key relationship

            // Optionally, you can configure additional properties or constraints here
            modelBuilder.Entity<Movie>()
             .HasIndex(m => m.MovieId)
             .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
