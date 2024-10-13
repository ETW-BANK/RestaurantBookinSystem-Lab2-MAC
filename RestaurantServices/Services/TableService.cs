
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.Services
{
    public class TableService : ITableService

    {
        private IUnitOfWork _unitOfWork;
       


        public TableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
      
        }

        public void CreateTable(TablesVM table)
        {
            var newTable = new Tables
            {
                TableNumber = table.TableNumber,
                NumberOfSeats = table.NumberOfSeats,
                IsAvailable = table.IsAvailable
            };
            _unitOfWork.Repository<Tables>().Add(newTable);
            _unitOfWork.Save();
        }

        public void DeleteTable(int id)
        {
            var tables = _unitOfWork.Repository<Tables>().GetById(id);

            _unitOfWork.Repository<Tables>().Delete(tables);
            _unitOfWork.Save();
        }

        public IEnumerable<TablesVM> GetAllTables()
        {
           
            var tablesVMs = _unitOfWork.Repository<Tables>()
                                       .GetAll()
                                       .Select(t => new TablesVM
                                       {
                                           Id = t.Id,
                                           TableNumber = t.TableNumber,
                                           NumberOfSeats = t.NumberOfSeats,
                                           IsAvailable = t.IsAvailable
                                       }).ToList();

            return tablesVMs;
        }


        public TablesVM GetById(int id)
        {
            var tables = _unitOfWork.Repository<Tables>().GetById(id);

            if (tables == null)
            {
                
                return null;
            }

          
            var table = new TablesVM
            {
                Id= tables.Id,
                TableNumber = tables.TableNumber,
                NumberOfSeats = tables.NumberOfSeats,
                IsAvailable = tables.IsAvailable
            };

            return table;
        }

        public void UpdateTable(TablesVM tableVM)
        {
           
            var existingTable = _unitOfWork.Repository<Tables>().GetById(tableVM.Id);

            if (existingTable == null)
            {
                throw new Exception("Table not found");
            }

            
            existingTable.TableNumber = tableVM.TableNumber;
            existingTable.NumberOfSeats = tableVM.NumberOfSeats;
            existingTable.IsAvailable = tableVM.IsAvailable;

         
            _unitOfWork.Repository<Tables>().Update(existingTable);


            _unitOfWork.Save();
        }

       
    }
}
