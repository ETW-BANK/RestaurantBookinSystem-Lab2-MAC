
using Restaurant.Models;
using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.Services.IServices
{
   public interface ITableService
    {
        IEnumerable<Tables> GetAllTables();

        Tables GetById(int id);

        void UpdateTable(Tables table);


        void CreateTable (Tables table);  

        void DeleteTable (int id);

    }
}
