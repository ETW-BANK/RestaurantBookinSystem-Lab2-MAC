﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services;
using Restaurant.Data.Access.Repository.Services.IServices;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;

namespace RetaurantBooking.Controllers
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
        public async Task<IActionResult> GetAllCategories()
        {
            // Pass scheme and host to the service method
            var scheme = HttpContext.Request.Scheme;
            var host = HttpContext.Request.Host.ToString();

            var categories = _categoryService.GetAllCategories(scheme, host);

            return Ok(categories);
        }

        [HttpPost]

        public async Task<IActionResult> CreateCategory([FromForm] CategoryVM category)
        {
            if (category == null)
            {
                return BadRequest(new { message = "Category data is missing!" });
            }

            try
            {
                await _categoryService.CreateCategory(category);
                return Ok(new { message = "Category created successfully", imageUrl = category.ImageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound("Category Not Found");
            }
            return Ok(category);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromQuery] CategoryVM category)
        {
            if (category == null)
            {
                return BadRequest("Invalid Category data");
            }

            if (category == null)
            {
                return NotFound("Category not found");
            }
            _categoryService.UpdateCategory(category);
            return Ok(new { message = "Category Updated successfully." });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var CategoryToDelete = _categoryService.GetById(id);
            if (CategoryToDelete == null)
            {
                return NotFound("Category Not Found");
            }


            _categoryService.DeleteCategory(CategoryToDelete);

            return Ok(new { message = "Category Deleted successfully." });
        }
    }
}