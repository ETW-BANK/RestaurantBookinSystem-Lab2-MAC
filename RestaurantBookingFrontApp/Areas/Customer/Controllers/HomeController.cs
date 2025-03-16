
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> Menues()
        {
            var menuelist = await _menuService.GetAll();

            if (menuelist == null)
            {
                TempData["error"] = "No menus found.";
                return View(menuelist);
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
