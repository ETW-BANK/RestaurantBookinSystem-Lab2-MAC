
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromForm] CategoryVM categoryVM,IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
               
                return BadRequest(ModelState);
            }

                await _categoryService.CreateCategory(categoryVM,file);
            
                return Ok("Category created successfully.");
            
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
        public async Task<IActionResult> UpdateCategory( CategoryVM category,IFormFile? file)
        {

            var categoryToUpdate = _categoryService.GetById(category.Id);
            if (category == null)
            {
                return BadRequest("Invalid Category data");
            }

            try
            {
               _categoryService.Update(category, file);
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