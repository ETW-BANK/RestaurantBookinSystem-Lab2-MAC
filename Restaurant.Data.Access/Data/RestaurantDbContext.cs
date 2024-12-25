
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

        public DbSet<BookingHeder> BookingHeders { get; set; }

        public DbSet<BookingDetail> BookingDetails { get; set; }
        public DbSet<MyBookings> MyBookings { get; set; }
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Tables)
                .WithMany()
                .HasForeignKey(b => b.TableId);
               

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ApplicationUser)
                .WithMany()
                .HasForeignKey(b => b.ApplicationUserId);
        }

    }
    }

