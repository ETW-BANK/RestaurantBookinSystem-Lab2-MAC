using System;

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

        //public async Task UpdateCategory(CategoryVM categoryVM)
        //{
        //    var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == categoryVM.Id);
        //    if (category != null)
        //    {
        //        category.Name = categoryVM.Name;
        //        category.Description = categoryVM.Description;
              
        //        _unitOfWork.CategoryRepository.Update(category);
        //        await _unitOfWork.SaveAsync();
        //    }
        //}

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

        public void CreateCategory(CategoryVM categoryVM, IFormFile? file)
        {
            if (categoryVM == null)
            {
                throw new ArgumentNullException(nameof(categoryVM), "CategoryVM is null");
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string categoryPath = Path.Combine(wwwRootPath, "image", "category");

                if (!Directory.Exists(categoryPath))
                {
                    Directory.CreateDirectory(categoryPath);
                }

              
                if (!string.IsNullOrEmpty(categoryVM.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, categoryVM.ImageUrl.TrimStart('/').Replace("/", "\\"));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

         
                using (var fileStream = new FileStream(Path.Combine(categoryPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                categoryVM.ImageUrl = $"/image/category/{fileName}";
            }

            var newCategory = new Category
            {
                Name = categoryVM.Name,
                DisplayOrder = categoryVM.DisplayOrder,
                ImageUrl = categoryVM.ImageUrl 
            };

            _unitOfWork.CategoryRepository.Add(newCategory);
            _unitOfWork.SaveAsync();
        }


    





    public Task UpdateCategory(CategoryVM category)
        {
            throw new NotImplementedException();
        }



    }

}
