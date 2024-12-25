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
    public class MybookingsRepository:Repository<MyBookings>,IMybookingsRepository
    {

        private readonly RestaurantDbContext _context;

        public MybookingsRepository(RestaurantDbContext context):base(context) 
        {
            _context = context;
        }

        public void update(MyBookings newBookings)
        {
            throw new NotImplementedException();
        }
    }
}
