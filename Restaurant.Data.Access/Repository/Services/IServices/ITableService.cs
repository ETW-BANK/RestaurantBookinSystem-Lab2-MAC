using Restaurant.Models.DTOs;
using Restaurant.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.Services.IServices
{
   public interface ITableService
    {
        Task<ServiceResponse<string>> AddItemAsync(TablesDto item);
        Task<ServiceResponse<TablesDto>> GetSingleAsync(int id);
        Task<ServiceResponse<IEnumerable<TablesDto>>> GetAllAsync();
        Task<ServiceResponse<bool>> RemoveAsync(int id);
        Task<ServiceResponse<bool>> UpdateTableAsync(TablesDto table);
    }
}
