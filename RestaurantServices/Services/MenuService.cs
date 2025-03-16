
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System;


namespace RestaurantServices.Services
{
    public class MenuService : IMenuService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MenuService(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Menue>> GetAll()
        {
            var categoryList = _unitOfWork.CategoryRepository.GetAll()
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }).ToList(); // Convert to List

            var menus =  _unitOfWork.MenuRepository.GetAll(includeProperties:"Category").ToList(); // Ensure async retrieval

            return menus;
        }


        public async Task<MenuVM?> GetbyId(int? id)
        {

            MenuVM menueVM = new()
            {
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

               Menue = new Menue()
            };

            if (id == null || id == 0)
            {
                return menueVM;
            }
            else
            {
               menueVM.Menue = _unitOfWork.MenuRepository.GetFirstOrDefault(u => u.menueId == id);

                return menueVM;
            }


        }



        public async Task Update(MenuVM menu, IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
    
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string menuPath = Path.Combine(wwwRootPath, @"image\menu");
          
                if (!Directory.Exists(menuPath))
                {
                    Directory.CreateDirectory(menuPath);
                }
               
                if (!string.IsNullOrEmpty(menu.Menue.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, menu.Menue.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
               
                using (var fileStream = new FileStream(Path.Combine(menuPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
          
                menu.Menue.ImageUrl = @"\image\menu\" + fileName;
            }

            var existingMenu = _unitOfWork.MenuRepository.GetFirstOrDefault(c => c.menueId == menu.Menue.menueId);

            if (menu.Menue.menueId !=0 )
            {
            
                existingMenu.Name = menu.Menue.Name;
                existingMenu.Description = menu.Menue.Description;
                existingMenu.ImageUrl = menu.Menue.ImageUrl;
                existingMenu.Price = menu.Menue.Price;
                existingMenu.Qty = menu.Menue.Qty;
                existingMenu.Available = menu.Available;
                existingMenu.CategoryId = menu.Menue.CategoryId;


                _unitOfWork.MenuRepository.UpdateMenu(menu.Menue);

             
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new InvalidOperationException("Menu not found.");
            }
        }
        public async Task CreateMenue(MenuVM menueVM, IFormFile? file)
        {
            string wwwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string menuPath = Path.Combine(wwwwRootPath, @"image\menu");

                if (!Directory.Exists(menuPath))
                {
                    Directory.CreateDirectory(menuPath);
                }

                using (var fileStream = new FileStream(Path.Combine(menuPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                menueVM.Menue.ImageUrl = @"\image\menu\" + fileName;
            }

            var menuExist = _unitOfWork.MenuRepository.GetFirstOrDefault(x => x.Name == menueVM.Menue.Name);
            if (menuExist != null)
            {
                throw new Exception("Menu already exists.");
            }

            IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            if (!categoryList.Any()) 
            {
                throw new Exception("Selected category not found.");
            }

            var menu = new Menue
            {
                menueId = menueVM.Menue.menueId,
                Name = menueVM.Menue.Name,
                Description = menueVM.Menue.Description,
                Price = menueVM.Menue.Price,
                Qty = menueVM.Menue.Qty,
                ImageUrl = menueVM.Menue.ImageUrl,
                Available = menueVM.Available,
                CategoryId = menueVM.Menue.CategoryId,
            
            };

            _unitOfWork.MenuRepository.Add(menu);
            await _unitOfWork.SaveAsync();
        }

       

        public void DeleteMenue(int? id)
        {
            var menutodelete = _unitOfWork.MenuRepository.GetFirstOrDefault(x => x.menueId == id);

            if (menutodelete == null)
            {
                throw new Exception("Menu not found.");
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(menutodelete.ImageUrl))
            {
                string oldPath = Path.Combine(wwwRootPath, menutodelete.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            _unitOfWork.MenuRepository.Remove(menutodelete);
            _unitOfWork.SaveAsync();

          
        }

      
    }

    }


    

