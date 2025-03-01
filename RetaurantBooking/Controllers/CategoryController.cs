using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;
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

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories =  _categoryService.GetAllCategories(); 
            if (categories == null )
            {
                return NotFound(new { message = "No Category found." });
            }

            return Ok(categories);
        }

        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryVM category)
        {
            try
            {
                await _categoryService.CreateCategory(category);
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }




        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound("Category Not Found");
            }
            return Ok(category);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryVM category)
        {
            if (category == null)
            {
                return BadRequest("Invalid Category data");
            }

            try
            {
                await _categoryService.UpdateCategory(category);
                return Ok(new { message = "Category updated successfully." });
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
            var categoryToDelete = _categoryService.GetById(id);
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