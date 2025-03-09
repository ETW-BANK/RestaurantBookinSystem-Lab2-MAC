using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using Restaurant.Services;
using RestaurantServices.Services;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System;
using System.Threading.Tasks;

namespace YourNamespace.Backend.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
       


        public CategoryController(ICategoryService categoryService, IWebHostEnvironment webHostEnvironment, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
          
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = _categoryService.GetAll();
            
            if (categories == null )
            {
                return NotFound(new { message = "No Category found." });
            }

            return Ok(categories);  
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryVM categoryVM, IFormFile?file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _categoryService.CreateCategory(categoryVM,file);
                    return Ok(categoryVM);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest(ModelState);
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