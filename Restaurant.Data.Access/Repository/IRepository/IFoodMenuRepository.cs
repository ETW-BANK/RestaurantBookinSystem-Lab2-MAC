using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.IRepository
{
   public interface IFoodMenuRepository:IRepository<FoodMenu>
    {
        Task UpdateMenuAsync(FoodMenu menu);
    }
}
