using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository
{
    public class FoodMenuRepository : Repository<FoodMenu>, IFoodMenuRepository
    {

        private readonly RestaurantDbContext _context;

        public FoodMenuRepository(RestaurantDbContext context):base(context) 
        {
            _context = context;
        }
       

        public async Task UpdateMenuAsync(FoodMenu menu)
        {
           var existingTable=await GetSingleAsync(menu.Id);

            if (existingTable!=null)
            {
                 _context.FoodMenus.Update(existingTable);  
            }
            await _context.SaveChangesAsync();
        }
    }
}
