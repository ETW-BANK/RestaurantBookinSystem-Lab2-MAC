
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantViewModels;



namespace Restaurant.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
      

        private readonly IConfiguration _configuration;
        public CategoryService(IUnitOfWork unitOfWork,IConfiguration configuration)
        {
      
            _unitOfWork= unitOfWork;
            _configuration = configuration;
        }

        public async Task CreateCategory(CategoryVM category)
        {
            
            var categoryExist = _unitOfWork.CategoryRepository.GetFirstOrDefault(x => x.Name == category.Name);
            if (categoryExist != null)
            {
                throw new Exception("Category already exists.");
            }

           
            var newCategory = new Category
            {
                Name = category.Name,
                DisplayOrder = category.DisplayOrder,
                ImageUrl = category.ImageUrl
            };

         
            _unitOfWork.CategoryRepository.Add(newCategory);

      
            await _unitOfWork.SaveAsync();
        }



        public Category DeleteCategory(Category category)
        {
          category = _unitOfWork.CategoryRepository.GetFirstOrDefault(x=>x.Id==category.Id);
          

            _unitOfWork.CategoryRepository.Remove(category);
            _unitOfWork.SaveAsync();

            return category;
        }

        public async Task< IEnumerable<CategoryVM>> GetAll()
        {
            var backendBaseUrl = _configuration["BackendBaseUrl"];
            if (string.IsNullOrEmpty(backendBaseUrl))
            {
                throw new InvalidOperationException("BackendBaseUrl is not configured.");
            }
            return _unitOfWork.CategoryRepository.GetAll().Select(c => new CategoryVM()
         {
             Id = c.Id,
             Name = c.Name,
             DisplayOrder = c.DisplayOrder,
             ImageUrl = $"{backendBaseUrl}{c.ImageUrl}"
            }).ToList();


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