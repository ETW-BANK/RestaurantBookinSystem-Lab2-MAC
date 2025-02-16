using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;

namespace RestaurantServices.Services
{
    public class MenuService : IMenuService
    {
        private IUnitOfWork _unitOfWork;

        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public void CreateMenu(MenuVM menu)
        {
            var category = _unitOfWork.CategoryRepository
                        .GetFirstOrDefault(c => c.Name == menu.CategoryName);
            if (category == null)
                throw new Exception("Category not found");

            var newMenu = new Menue
            {
                Name = menu.Name,
                Description = menu.Description,
                Price = menu.Price,
                Qty = menu.Qty,
                Image = menu.Image,
                CategoryId = category.Id,
            };

            if (menu.Qty > 0)
                newMenu.Available = Available.Yes;
            else
                newMenu.Available = Available.NO;

            _unitOfWork.MenuRepository.Add(newMenu);
            _unitOfWork.Save();
        }

        public Menue DeleteMenu(Menue menu)
        {
            var menuToDelete = _unitOfWork.MenuRepository.GetFirstOrDefault(u => u.menueId == menu.menueId, includeProperties: "Category");    

            if (menuToDelete == null)
            {
                throw new Exception("Menu not found");
            }

            _unitOfWork.MenuRepository.Remove(menuToDelete);
            _unitOfWork.Save();

            return menuToDelete;

        }

        public IEnumerable<MenuVM> GetAllMenues()
        {
           var menuList = _unitOfWork.MenuRepository.GetAll(includeProperties: "Category");

            var result = menuList.Select(m => new MenuVM
            {
                MenueId = m.menueId,    
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                Qty = m.Qty,
                Image = m.Image,
                CategoryName = m.Category.Name,
                AvailableStatus = m.Available.ToString()


            }).ToList();

            return result;    
        }

        public Menue GetById(int id)
        {
           var menu = _unitOfWork.MenuRepository.GetFirstOrDefault(u => u.menueId == id, includeProperties: "Category");

            if (menu == null)
            {
                throw new Exception("Menu not found");
            }
               
            return menu;   

           
        }

        public void UpdateMenu(MenuVM menu)
        {
            var menueToUpdate = _unitOfWork.MenuRepository.GetFirstOrDefault(u => u.menueId == menu.MenueId);

            if (menueToUpdate == null)
            {
                throw new Exception("Menu not found");
            }

            var category = _unitOfWork.CategoryRepository
                            .GetFirstOrDefault(c => c.Name == menu.CategoryName);

            if (category == null)
            {
                throw new Exception("Category not found");
            }

            menueToUpdate.Name = menu.Name;
            menueToUpdate.Description = menu.Description;
            menueToUpdate.Price = menu.Price;
            menueToUpdate.Qty = menu.Qty;
            menueToUpdate.Image = menu.Image;
            menueToUpdate.CategoryId = category.Id;

            menueToUpdate.Available = (menu.Qty > 0) ? Available.Yes : Available.NO;

            _unitOfWork.MenuRepository.UpdateMenu(menueToUpdate); 
            _unitOfWork.Save();
        }

    }
}
