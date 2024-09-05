using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Data
{
   public class RestaurantDbContext:DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FoodMenu>? FoodMenus { get; set; }
        public DbSet<Tables> Table { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options):base(options) 
        {
            
        }
    }
}
