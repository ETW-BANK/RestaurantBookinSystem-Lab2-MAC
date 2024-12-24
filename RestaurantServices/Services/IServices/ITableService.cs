
using Restaurant.Models;
using RestaurantViewModels;


namespace Restaurant.Data.Access.Repository.Services.IServices
{
   public interface ITableService
    {
        IEnumerable<Tables> GetAllTables();

        Tables GetById(int id);

        void UpdateTable(TablesVM table);


        void CreateTable (TablesVM table);  

        void DeleteTable (int id);

    }
}
