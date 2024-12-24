

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using RestaurantViewModels;

namespace Restaurant.Data.Access.Repository.Services
{
    public class TableService : ITableService

    {
        private IUnitOfWork _unitOfWork;

        private readonly Random _random = new Random();

        public TableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
      
        }



        public void CreateTable(TablesVM tableVM)
        {
        
            int tableNumber;
            do
            {
                tableNumber = _random.Next(1, 30);
            }
            while (_unitOfWork.TableRepository.GetFirstOrDefault(t => t.TableNumber == tableNumber) != null);

            
            var table = new Tables
            {
                TableNumber = tableNumber,
                NumberOfSeats = tableVM.NumberOfSeats,
                IsAvailable = true 
            };

          
            _unitOfWork.TableRepository.Add(table);
            _unitOfWork.Save();
        }






        public void DeleteTable(int id)
        {
            var table = _unitOfWork.TableRepository.GetFirstOrDefault(t => t.Id == id);
            if (table == null)
            {
                throw new Exception("Table not found");
            }

            _unitOfWork.TableRepository.Remove(table);
            _unitOfWork.Save();
        }

        public IEnumerable<Tables> GetAllTables()
        {
           
            var tablelist=_unitOfWork.TableRepository.GetAll().ToList();    

            return tablelist;
        }


        public Tables GetById(int id)
        {
            var table = _unitOfWork.TableRepository.GetFirstOrDefault(u => u.Id == id);
            return table ?? throw new Exception("Table not found");
        }

        public void UpdateTable(TablesVM tableVM)
        {
           
            var existingTable = _unitOfWork.TableRepository.GetFirstOrDefault(u => u.Id == tableVM.Id);

            if (existingTable == null)
            {
                throw new Exception("Table not found");
            }

           existingTable.TableNumber=tableVM.TableNumber;   
            existingTable.NumberOfSeats = tableVM.NumberOfSeats;
            existingTable.IsAvailable = tableVM.IsAvailable;

            _unitOfWork.TableRepository.UpdateTable(existingTable);
            _unitOfWork.Save();
        }




    }
}
