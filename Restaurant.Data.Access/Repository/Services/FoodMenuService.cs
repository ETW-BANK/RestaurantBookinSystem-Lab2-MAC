using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using Restaurant.Models.DTOs;
using Restaurant.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.Services
{
    public class FoodMenuService : IFoodMenuService
    {

        private readonly IFoodMenuRepository _foodMenurepo;

        public FoodMenuService(IFoodMenuRepository foodMenurepo)
        {
            _foodMenurepo = foodMenurepo;
        }
        public async Task<ServiceResponse<string>> AddItemAsync(FoodMenuDto item)
        {
           var response=new ServiceResponse<string>();

            try
            {
                var newmenu = new FoodMenu
                {
                    Id = item.Id,
                    Title = item.Title,
                    Price=item.price,
                    ImageUrl = item.ImageUrl,
                    IsAvailable = item.IsAvailable,
                };


                await _foodMenurepo.AddItemAsync(newmenu);
                await _foodMenurepo.SaveAsync();
                response.Success = true;
                response.Message = Messages.MenuSucces;
            }
            catch
            {
                response.Success = false;
                response.Message = Messages.MenuFailed;
            }


            return response;

        }

        public async Task<ServiceResponse<IEnumerable<FoodMenuDto>>> GetAllAsync()
        {
            var response = new ServiceResponse<IEnumerable<FoodMenuDto>>();

            try
            {
                var menus = await _foodMenurepo.GetAllAsync();

                if (menus != null)
                {

                    var menulist = menus.Select(u => new FoodMenuDto
                    {
                        Id = u.Id,
                        Title = u.Title,
                        price = u.Price,    
                        ImageUrl = u.ImageUrl,
                        IsAvailable = u.IsAvailable,
                    }).ToList();

                    response.Success = true;    
                  
                    response.Data = menulist;
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
                response.Data=Enumerable.Empty<FoodMenuDto>();
            }

            return response;
        }

        public async Task<ServiceResponse<FoodMenuDto>> GetSingleAsync(int id)
        {
            var response = new ServiceResponse<FoodMenuDto>();

            try
            {
                var menue = await _foodMenurepo.GetSingleAsync(id);

                if (menue != null)
                {
                    var singlemenue = new FoodMenuDto
                    {
                        Id = menue.Id,
                        Title = menue.Title,
                        price=menue.Price,
                        ImageUrl = menue.ImageUrl,
                        IsAvailable = menue.IsAvailable,
                    };
                    response.Success = true;
                  
                    response.Data = singlemenue;
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
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveAsync(int id)
        {
            var responce=new ServiceResponse<bool>(); 
            
            try
            {
                var foodtodelete = await _foodMenurepo.GetSingleAsync(id);

                if(foodtodelete != null)
                {
                    await _foodMenurepo.RemoveAsync(id);
                    await _foodMenurepo.SaveAsync();

                    responce.Success = true;
                    responce.Message=Messages.DataDelete;
                }

                else
                {
                    responce.Success = false;
                    responce.Message = Messages.DFailDelete;
                }
            }
            catch
            {
                responce.Success=false;
                responce.Message=Messages.NoData;
            }

            return responce;
        }

        public async Task<ServiceResponse<bool>> UpdateMenueAsync(FoodMenuDto table)
        {
            var responce =new  ServiceResponse<bool>();

            try
            {
                var existingmenue = await _foodMenurepo.GetSingleAsync(table.Id);

                if(existingmenue != null)
                {
                    existingmenue.Id = table.Id;
                    existingmenue.Title = table.Title;
                    existingmenue.Price = table.price;
                    existingmenue.ImageUrl = table.ImageUrl;
                    existingmenue.IsAvailable = table.IsAvailable;

                    await _foodMenurepo.UpdateMenuAsync(existingmenue);
                    responce.Success = true;
                    responce.Message = Messages.MenuUpdateSucces;

                }

                else
                {
                    responce.Success = false;
                    responce.Message=Messages.NoData;

                }
            }
            catch
            {
                responce.Success=false;
                responce.Message=Messages.MenuFailed;
            }

            return responce ;
        }
    }

       
    }

