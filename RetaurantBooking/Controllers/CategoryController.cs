
using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;
using Restaurant.Services;
using RestaurantViewModels;



namespace YourNamespace.Backend.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
       
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(ICategoryService categoryService, IWebHostEnvironment webHostEnvironment, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAll();
            
            if (categories == null )
            {
                return NotFound(new { message = "No Category found." });
            }

            return Ok(categories);  
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
               
                return BadRequest(ModelState);
            }

            try
            {
                if (categoryVM.Image != null)
                {
                   
                    string folder = @"images/category/";
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + categoryVM.Image.FileName;
                    string filePath = Path.Combine(folder, uniqueFileName);

                   
                    categoryVM.ImageUrl ="/"+ filePath;

                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    if (!Directory.Exists(serverFolder))
                    {
                        Directory.CreateDirectory(serverFolder);
                    }

                    string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await categoryVM.Image.CopyToAsync(fileStream);
                    }
                }

              
                await _categoryService.CreateCategory(categoryVM);

            
                return Ok("Category created successfully.");
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        [HttpGet("{id}")]
        public async Task< IActionResult> GetCategory(int id)
        { 

          try
            {
               var category= _categoryService.GetById(id);

                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory( Category category)
        {

            var categoryToUpdate = _categoryService.GetById(category.Id);
            if (category == null)
            {
                return BadRequest("Invalid Category data");
            }

            try
            {
               _categoryService.Update(category);
                return Ok("Category updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating category: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while updating the category.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var categoryToDelete =_categoryService.GetById(id);  
            if (categoryToDelete == null)
            {
                return NotFound("Category Not Found");
            }

            try
            {
                _categoryService.DeleteCategory(categoryToDelete);
                return Ok(new { message = "Category deleted successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting category: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while deleting the category.", details = ex.Message });
            }
        }
    }
}