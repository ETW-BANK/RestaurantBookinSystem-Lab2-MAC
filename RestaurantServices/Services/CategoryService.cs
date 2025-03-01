using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            var table = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id);
            return table ?? throw new Exception("Category not found");
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
            var newCategory = new Category
            {
                
                
                Id = categoryVM.Id,
                Name = categoryVM.Name,
                Description = categoryVM.Description,
                ImageUrl = categoryVM.ImageUrl,

            };
            _unitOfWork.CategoryRepository.Add(newCategory);
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<CategoryVM> GetAllCategories()
        {
            var categorylist = _unitOfWork.CategoryRepository.GetAll();

            return categorylist.Select(t => new CategoryVM
            {
               
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,    
                    ImageUrl = t.ImageUrl,  



            }).ToList();
        }
    }
       
    }
