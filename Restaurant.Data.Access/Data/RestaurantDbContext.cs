
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


        }
    }


