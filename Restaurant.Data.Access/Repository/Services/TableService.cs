using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using Restaurant.Models.DTOs;
using Restaurant.Utility;
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
        private readonly ITableRepository _tablerepo;

        public TableService(ITableRepository tablerepo)
        {
            _tablerepo = tablerepo;
        }

        public async Task<ServiceResponse<string>> AddItemAsync(TablesDto item)
        {
            var response = new ServiceResponse<string>();

            try
            {
                var newtable = new Tables
                {
                    Id = item.Id,
                    NumberOfSeats = item.NumberOfSeats,
                    TableNumber = item.TableNumber,
                    isAvialable = item.isAvialable
                };
                await _tablerepo.AddItemAsync(newtable);
                await _tablerepo.SaveAsync();
                response.Success = true;
                response.Message = Messages.TableSucces;

            }
            catch
            {
                response.Success = false;
                response.Message = Messages.TableFailed;
            }

            return response;

        }

        public async Task<ServiceResponse<IEnumerable<TablesDto>>> GetAllAsync()
        {
            var response = new ServiceResponse<IEnumerable<TablesDto>>();

            try
            {
              var tables=  await _tablerepo.GetAllAsync();

                if (tables != null)
                {
                    var tableList = tables.Select(u => new TablesDto
                    {
                        Id = u.Id,
                        TableNumber = u.TableNumber,
                        NumberOfSeats = u.NumberOfSeats,
                        isAvialable = u.isAvialable
                    }).ToList();

                    response.Data = tableList;
                    response.Success = true;

                }
                else
                {

                    response.Success = false;
                    response.Message = Messages.NoData;
                }

              
            }
            catch
            {
                response.Success = false;
                response.Message = Messages.NoData;
                response.Data = Enumerable.Empty<TablesDto>();
            }

            return response;
        }

        public async Task<ServiceResponse<TablesDto>> GetSingleAsync(int id)
        {
            var response = new ServiceResponse<TablesDto>();
            try
            {
                var tabel= await _tablerepo.GetSingleAsync(id);    

                if (tabel != null)
                {
                    var tables = new TablesDto
                    {
                        Id = tabel.Id,
                        TableNumber = tabel.TableNumber,
                        NumberOfSeats = tabel.NumberOfSeats,
                        isAvialable = tabel.isAvialable
                    };

                    response.Data=tables;    
                    response.Success = true;
                    response.Message = Messages.TableRetrive;

                }
                else
                {
                    response.Success = false;
                    response.Message = Messages.NoData;
                }

            }
            catch
            {
                response.Success= false;
                response.Message = Messages.NoData;
                
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveAsync(int id)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var table = await _tablerepo.GetSingleAsync(id);

                if(table != null)
                {
                    await _tablerepo.RemoveAsync(id);
                    await _tablerepo.SaveAsync();
                    response.Data = true;
                    response.Success = true;
                    response.Message = Messages.DataDelete;
                }
                else
                {
                    response.Success = false;
                    response.Message = Messages.DFailDelete;
                }

            }
            catch
            {
                response.Success= false;
                response.Message = Messages.DFailDelete;
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateTableAsync(TablesDto table)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var existingtabe = await _tablerepo.GetSingleAsync(table.Id);

                if (existingtabe != null) 
                {
                   
                    existingtabe.Id = table.Id;
                    existingtabe.TableNumber = table.TableNumber;
                    existingtabe.NumberOfSeats= table.NumberOfSeats;
                    existingtabe.isAvialable = table.isAvialable;


                    await _tablerepo.UpdateTableAsync(existingtabe);

                    response.Data = true;
                    response.Success = true;
                    response.Message = Messages.TableUpdateSucces;


                }
                else
                {
                    response.Data = false;
                    response.Success = false;
                    response.Message = Messages.TableUpdateFailed;
                }
            }
            catch
            {
                response.Data = false;
                response.Success= false;
                response.Message = Messages.TableUpdateFailed;
            }

            return response;
        }
    }
}
