using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Access.Data;
using Restaurant.Models;
using Restaurant.Utility;


namespace Restaurant.Data.Access.DbInisializer
{
    public class DbInitializer:IDbInitilizer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RestaurantDbContext _context;
        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,RestaurantDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
           _context = context;
        }
        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }

            }
            catch (Exception ex)
            {

            }

            if (!_roleManager.RoleExistsAsync(StaticData.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticData.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticData.Role_Admin)).GetAwaiter().GetResult();


                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@green.com",
                    Email = "admin@green.com",
                    Name = "Tense Girma",
                    StreetAddress = "Ellinsborgsbacken 22",
                    State = "Spånga",
                    PostalCode = "16364",
                    City = "Stockholm"
                }, "@Admin12345").GetAwaiter().GetResult();

                ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@40green.com");
                _userManager.AddToRoleAsync(user, StaticData.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
