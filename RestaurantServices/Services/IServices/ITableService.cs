
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
        IEnumerable<TablesVM> GetAllTables();

        TablesVM GetById(int id);

        void UpdateTable(TablesVM table);


        void CreateTable (TablesVM table);  

        void DeleteTable (int id);

    }
}
