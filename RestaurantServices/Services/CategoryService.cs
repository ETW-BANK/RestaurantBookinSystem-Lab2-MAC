
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantViewModels;



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

        public async Task CreateCategory(CategoryVM category,IFormFile? file)
        {
            string wwwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {

                string fileNmae = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string categoryPath = Path.Combine(wwwwRootPath, @"image\category");

                if (!Directory.Exists(categoryPath))
                {
                    Directory.CreateDirectory(categoryPath);
                }

                using (var fileStream = new FileStream(Path.Combine(categoryPath, fileNmae), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                category.ImageUrl = @"\image\category\" + fileNmae;
            }


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

      

        public async Task< IEnumerable<CategoryVM>> GetAll()
        {
          
            return   _unitOfWork.CategoryRepository.GetAll().Select(c => new CategoryVM()
         {
             Id = c.Id,
             Name = c.Name,
             DisplayOrder = c.DisplayOrder,
             ImageUrl = c.ImageUrl,
            
            }).ToList();


        }
        public Category GetMenuCategory(int? id)
        {
            var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id,includeProperties:"Menues");

            return category;
        }
        public Category GetById(int? id)
        {
           var category = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

            return category;    
        }

        public void Update(CategoryVM category,IFormFile?file)
        {

            string wwwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {

                string fileNmae = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string categoryPath = Path.Combine(wwwwRootPath, @"image\category");

                if(string.IsNullOrEmpty(category.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwwRootPath, category.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
               

                using (var fileStream = new FileStream(Path.Combine(categoryPath, fileNmae), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                category.ImageUrl = @"\image\category\" + fileNmae;
            }
            var existingcateogry = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == category.Id);

            if (existingcateogry != null)
            {
                existingcateogry.Name = category.Name;
                existingcateogry.DisplayOrder = category.DisplayOrder;
                existingcateogry.ImageUrl = category.ImageUrl;

                _unitOfWork.CategoryRepository.Update(existingcateogry);
                _unitOfWork.SaveAsync();
            }
        }

        public async Task<Category> DeleteCategory(Category category)
        {
           
            category = _unitOfWork.CategoryRepository.GetFirstOrDefault(
                x => x.Id == category.Id,
                includeProperties: "Menues" 
            );

            if (category == null)
            {
                throw new Exception("Category not found.");
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                string oldPath = Path.Combine(wwwRootPath, category.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

         
            if (category.Menues != null && category.Menues.Any())
            {
                _unitOfWork.MenuRepository.RemoveRange(category.Menues);
            }

        
            _unitOfWork.CategoryRepository.Remove(category);

           
            await _unitOfWork.SaveAsync();

            return category;
        }
    }
}