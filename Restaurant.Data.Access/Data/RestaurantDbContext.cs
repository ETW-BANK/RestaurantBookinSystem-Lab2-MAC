using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Restaurant.Data.Access.Data
{
   public class RestaurantDbContext:IdentityDbContext<IdentityUser>
    {
      
        public DbSet<Tables> Table { get; set; }
       public DbSet<ApplicationUser> ApplicationUsers { get; set; } 

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options):base(options) 
        {
            
        }
    }
}
