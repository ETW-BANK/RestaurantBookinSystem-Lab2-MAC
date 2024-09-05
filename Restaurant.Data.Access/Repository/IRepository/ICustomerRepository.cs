using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface ICustomerRepository:IRepository<Customer>
    {
       

        Task UpdateCustomerAsync(Customer customer);
    }
}
