using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using Restaurant.Models.DTOs;
using Restaurant.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.Services.IServices
{
    public interface ICustomerService
    {

        Task <ServiceResponse<string>> AddItemAsync(CustomerDto item);
        Task<ServiceResponse<CustomerDto>> GetSingleAsync(int id);
        Task<ServiceResponse<IEnumerable<CustomerDto>>> GetAllAsync();
        Task<ServiceResponse<bool>> RemoveAsync(int id);
        Task<ServiceResponse<bool>> UpdateCustomerAsync(CustomerDto customer);

    }
}
