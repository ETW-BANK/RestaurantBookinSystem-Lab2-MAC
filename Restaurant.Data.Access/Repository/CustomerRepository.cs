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
    public class CustomerRepository:Repository<Customer>,ICustomerRepository
    {
        private readonly RestaurantDbContext _context;
        public CustomerRepository(RestaurantDbContext context):base(context)
        {
            _context = context;
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {

            var existingcustomer=_context.Customers.FirstOrDefault(u=>u.Id==customer.Id);

            

            if (existingcustomer != null)
            {
             
             _context.Customers.Update(existingcustomer);
            }
            await _context.SaveChangesAsync();  
        }
    }
}
