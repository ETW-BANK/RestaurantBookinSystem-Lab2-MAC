using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models.DTOs;
using Restaurant.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.Services.IServices
{
 public interface IFoodMenuService
    {
        Task<ServiceResponse<string>> AddItemAsync(FoodMenuDto item);
        Task<ServiceResponse<FoodMenuDto>> GetSingleAsync(int id);
        Task<ServiceResponse<IEnumerable<FoodMenuDto>>> GetAllAsync();
        Task<ServiceResponse<bool>> RemoveAsync(int id);
        Task<ServiceResponse<bool>> UpdateMenueAsync(FoodMenuDto table);
    }
}
