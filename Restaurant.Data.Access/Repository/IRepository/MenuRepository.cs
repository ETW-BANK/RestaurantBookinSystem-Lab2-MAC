using Restaurant.Data.Access.Data;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.IRepository
{
   public class MenuRepository:Repository<category>,IMenuRepository
    {
        private readonly RestaurantDbContext _db;
        public MenuRepository(RestaurantDbContext db):base(db)
        {
            _db = db;
        }

        public void UpdateMenu(category menue)
        {
          _db.Update(menue);    
        }
    }

}
