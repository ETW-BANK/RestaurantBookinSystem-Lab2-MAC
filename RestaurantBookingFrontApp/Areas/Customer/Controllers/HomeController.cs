
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Models;
using Restaurant.Services;
using RestaurantBookingFrontApp.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System.Diagnostics;



namespace RestaurantBookingFrontApp.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
    
        private readonly ICategoryService _categoryService;
        private readonly IMenuService _menuService;
        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService,IMenuService menuService)
        {
            _logger = logger;
      
       
            _categoryService = categoryService;
            _menuService = menuService; 
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
        public async Task<IActionResult> Menues(int categoryId)
        {
            IEnumerable<Menue> menuelist;

            if (categoryId > 0)
            {
               
                var category = _categoryService.GetMenuCategory(categoryId);

                if (category == null)
                {
                    TempData["error"] = "Category not found.";
                    return RedirectToAction(nameof(Index));
                }

                menuelist = category.Menues;
            }
            else
            {
             
                TempData["error"] = "Invalid category ID.";
                menuelist = Enumerable.Empty<Menue>();
            }

            if (menuelist == null || !menuelist.Any())
            {
                TempData["error"] = "No menus found for the selected category.";
            }

            return View(menuelist);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
