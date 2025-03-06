using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using Restaurant.Utility;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;

namespace RestaurantServices.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }




        public Category GetById(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id);
            return category;
        }

        public async Task UpdateCategory(CategoryVM categoryVM)
        {
            var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == categoryVM.Id);
            if (category != null)
            {
                category.Name = categoryVM.Name;
                category.Description = categoryVM.Description;
              
                _unitOfWork.CategoryRepository.Update(category);
                await _unitOfWork.SaveAsync();
            }
        }

        public void DeleteCategory(Category category)
        {
         var categorytodelete=_unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == category.Id);

          
            _unitOfWork.CategoryRepository.Remove(categorytodelete);
            _unitOfWork.SaveAsync();
        }

       
        public IEnumerable<Category> GetAllCategories()
        {
            var categories = _unitOfWork.CategoryRepository.GetAll();

         
            return categories;  
        }

        public async Task<Category> CreateCategory(CategoryVM categoryVM)
        {
            string stringFile = UploadFile(categoryVM);

            var category = new Category
            {
            
                Name = categoryVM.Name,
                Description = categoryVM.Description,
                ImageUrl = stringFile
            };

             _unitOfWork.CategoryRepository.Add(category);
            await _unitOfWork.SaveAsync();

            return category; 
        }

       
        private string UploadFile(CategoryVM categoryVM)
        {
            string fileName= null;

            if (categoryVM.Image != null)
            {
                
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                fileName = Guid.NewGuid().ToString() + "_" + categoryVM.Image.FileName;
                string filePath=Path.Combine(uploadDir, fileName);
           

                using (var fileStream = new FileStream(filePath,FileMode.Create))
                {
                    categoryVM.Image.CopyTo(fileStream);
                }

            }

            return fileName;
        }

    }

}
