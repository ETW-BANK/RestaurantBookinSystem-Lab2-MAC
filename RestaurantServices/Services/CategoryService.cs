using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantServices.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }

        public void CreateCategory(CategoryVM category)
        {
            var newCategory = new Category
            {
                Name = category.Name,
                Description = category.Description
            }; 
            
            _unitOfWork.CategoryRepository.Add(newCategory);
            _unitOfWork.Save(); 
        }

        public Category DeleteCategory(Category category)
        {
           var categoryToDelete = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == category.Id);

            if (categoryToDelete == null)
            {
                throw new Exception("Category not found");
            }

            _unitOfWork.CategoryRepository.Remove(categoryToDelete);
            _unitOfWork.Save();

            return categoryToDelete;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var categoyList = _unitOfWork.CategoryRepository.GetAll();

            return categoyList;
        }

        public Category GetById(int id)
        {
            var catagorytofind = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id);

            if (catagorytofind == null)
            {
                throw new Exception("Category not found");
            }

            return catagorytofind;  
        }

        public void UpdateCategory(CategoryVM category)
        {
            var existingCategorye = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == category.Id);

            if (existingCategorye == null)
            {
                throw new Exception("Category not found");
            }

           existingCategorye.Name = category.Name;
           existingCategorye.Description = category.Description;
           

            _unitOfWork.CategoryRepository.Update(existingCategorye);
            _unitOfWork.Save();
        }
    }
}
