
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;

namespace Restaurant.Data.Access.Data
{
    public class RestaurantDbContext : IdentityDbContext
    {
        public DbSet<Tables> Table { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Menue> Menues { get; set; }
        public DbSet<Category> Categories { get; set; }

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedCategories(modelBuilder);
        }

        private void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1,DisplayOrder=1, Name = "Appetizers", Description = "", ImageUrl = "/images/category/appitaizers.jpeg" },
                new Category { Id = 2, DisplayOrder = 2, Name = "Main Courses", Description = "", ImageUrl = "/images/category/main.jpg"},
                new Category {Id = 3, DisplayOrder = 3, Name = "Desserts", Description = "", ImageUrl = "/images/category/desert.jpg" },
                 new Category {Id = 4, DisplayOrder = 4, Name = "Drinks", Description = "", ImageUrl = "/images/category/drinks.jpg" }
            );
        }
    }
    }


