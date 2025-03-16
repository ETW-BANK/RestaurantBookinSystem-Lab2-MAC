using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Services;
using Restaurant.Utility;
using RestaurantViewModels;


namespace RestaurantBookingFrontApp.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]
    public class CategoriesController : Controller
    {

        private readonly ICategoryService _categoryService;
      

        public CategoriesController(ICategoryService categoryService)
        {

            _categoryService = categoryService;
        
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {


            var categorylist = await _categoryService.GetAll();


            if (categorylist == null)
            {

                TempData["error"] = "An error occurred while retrieving categories.";
            }


            return View(categorylist);

        }
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            CategoryVM categoryVM = new();

            if (id == null || id == 0)
            {
                
                return View(categoryVM);
            }
            else
            {
                
                var category = _categoryService.GetById(id);
                if (category == null)
                {
                    return NotFound(); 
                }
             
                categoryVM.Id = category.Id;
                categoryVM.Name = category.Name;
                categoryVM.DisplayOrder = category.DisplayOrder;
                categoryVM.ImageUrl = category.ImageUrl;

                return View(categoryVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(CategoryVM categoryVM,IFormFile?file)
        {
           
            try
            {
                if (categoryVM.Id == 0)
                {
                  
                    await _categoryService.CreateCategory(categoryVM,file);
                    TempData["success"] = "Category created successfully.";
                }
                else
                {
                    
                     _categoryService.Update(categoryVM,file);

                    TempData["success"] = "Category updated successfully.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
                return View(categoryVM);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var category = _categoryService.GetById(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            var category = _categoryService.GetById(id);

            if (category == null)
            {
                TempData["error"] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }
           await _categoryService.DeleteCategory(category); 
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}

        
    
