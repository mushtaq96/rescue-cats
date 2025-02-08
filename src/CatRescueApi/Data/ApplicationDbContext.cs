using Microsoft.EntityFrameworkCore;
using CatRescueApi.Models;

namespace CatRescueApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cat> Cats { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Adoption> Adoptions { get; set; }
    }
}