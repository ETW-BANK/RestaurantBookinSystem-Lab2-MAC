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

        public async Task CreateCategory(CategoryVM categoryVM)
        {
            string stringFilenae= UploadFile(categoryVM);

            var category = new Category
            {
                Name = categoryVM.Name,
                Description = categoryVM.Description,
                DisplayOrder = categoryVM.DisplayOrder,
                ImageUrl = stringFilenae
            };


            _unitOfWork.CategoryRepository.Add(category);
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<CategoryVM> GetAllCategories()
        {
            var categorylist = _unitOfWork.CategoryRepository.GetAll();


            return categorylist.ToList().Select(c => new CategoryVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                DisplayOrder = c.DisplayOrder,
                ImageUrl = c.ImageUrl 
            });
        }

        private string UploadFile(CategoryVM categoryVM)
        {
            if (categoryVM.Image != null)
            {
                // Generate unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoryVM.Image.FileName);

                // Set path to save image
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/category");
                var filePath = Path.Combine(directoryPath, fileName);

                // Ensure directory exists
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    categoryVM.Image.CopyTo(stream);
                }

                // Return relative path for database storage
                return "/images/category/" + fileName;
            }

            // Return default image if no file is uploaded
            return "/images/category/default.jpg";
        }

    }

}
