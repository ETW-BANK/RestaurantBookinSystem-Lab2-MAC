using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryService(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
      
            _unitOfWork= unitOfWork;
            _webHostEnvironment = webHostEnvironment;   

        }

        public void CreateCategory(CategoryVM category,IFormFile? file)
        {

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string FileName=Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);  
                string categoryPath = Path.Combine(wwwRootPath, @"image\category");

                using(var fileStream = new FileStream(Path.Combine(categoryPath, FileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                
                category.Category.ImageUrl = @"/image/category/" + FileName;  


            }
             

            var categoryExist = _unitOfWork.CategoryRepository.GetFirstOrDefault(x => x.Name == category.Category.Name);

            if (categoryExist != null)
            {
                throw new Exception("Category already exists.");
            }

            var newCategory = new Category
            {
                Name = category.Category.Name,
                DisplayOrder = category.Category.DisplayOrder,
                ImageUrl = category.Category.ImageUrl
            };

            _unitOfWork.CategoryRepository.Add(newCategory);
            _unitOfWork.SaveAsync();
        }
    


    public Category DeleteCategory(Category category)
        {
          category = _unitOfWork.CategoryRepository.GetFirstOrDefault(x=>x.Id==category.Id);
          

            _unitOfWork.CategoryRepository.Remove(category);
            _unitOfWork.SaveAsync();

            return category;
        }

        public IEnumerable<Category> GetAll()
        {
          var categorylist=  _unitOfWork.CategoryRepository.GetAll();

            return categorylist;
        }

        public Category GetById(int id)
        {
           var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

            return category;    
        }

        public void Update(Category category)
        {
            var existingcateogry = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == category.Id);

            if (existingcateogry != null)
            {
                existingcateogry.Name = category.Name;
                existingcateogry.DisplayOrder = category.DisplayOrder;
                _unitOfWork.CategoryRepository.Update(existingcateogry);
                _unitOfWork.SaveAsync();
            }
        }
    }
}