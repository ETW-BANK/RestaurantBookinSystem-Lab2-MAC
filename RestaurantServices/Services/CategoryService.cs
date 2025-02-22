using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;

using RestaurantServices.Services.IServices;
using RestaurantViewModels;

namespace RestaurantServices.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public async Task CreateCategory(CategoryVM category)
        {
            try
            {
                if (category.ImageFile != null && category.ImageFile.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(category.ImageFile.FileName);
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\category");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Save the image file
                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        await category.ImageFile.CopyToAsync(fileStream);
                    }

                    // Set the correct image URL (this will be the URL that can be accessed by front-end)
                    category.ImageUrl = $"/images/category/{fileName}";
                }

                var newCategory = new Category
                {
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl
                };

                _unitOfWork.CategoryRepository.Add(newCategory);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                // Log the error (you can log it to a file or console or use a logging library)
                Console.WriteLine($"Error while uploading image: {ex.Message}");
                throw; // Rethrow the exception or handle as necessary
            }
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

        public IEnumerable<Category> GetAllCategories(string scheme, string host)
        {
            var categoryList = _unitOfWork.CategoryRepository.GetAll();

            foreach (var category in categoryList)
            {
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    // Ensure the path uses forward slashes (/) for web compatibility
                    category.ImageUrl = category.ImageUrl.Replace("\\", "/");

                    // If you need a full URL for API clients, construct it properly
                    category.ImageUrl = $"{scheme}://{host}{category.ImageUrl}";
                }
            }

            return categoryList;
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

        public async Task UpdateCategory(CategoryVM category)
        {
            var existingCategorye = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == category.Id);

            if (category.ImageFile != null && category.ImageFile.Length > 0)
            {

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(category.ImageFile.FileName);
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\category");

                if (!string.IsNullOrEmpty(existingCategorye.ImageUrl)   )
                {
                    var oldimagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingCategorye.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldimagePath))
                    {
                        System.IO.File.Delete(oldimagePath);
                    }
                }




                using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                {
                    await category.ImageFile.CopyToAsync(fileStream);

                    category.ImageUrl = $"/images/category/{fileName}";
                }


            }

            if (existingCategorye == null)
            {
                throw new Exception("Category not found");
            }

           existingCategorye.Name = category.Name;
           existingCategorye.Description = category.Description;
            existingCategorye.ImageUrl = category.ImageUrl; 


            _unitOfWork.CategoryRepository.Update(existingCategorye);
            _unitOfWork.Save();
        }
    }
}
