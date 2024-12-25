using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Models;
using Restaurant.Utility;
using RestaurantBookingFrontApp.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace RestaurantBookingFrontApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    
    public class HomeController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;
      
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/Booking/");
     
        }

        public IActionResult Index()
        {
            
            return View();
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var claimsIdentity = User?.Identity as ClaimsIdentity;

            if (claimsIdentity == null || !User.Identity.IsAuthenticated)
            {
                TempData["error"] = "User is not authenticated.";
                return RedirectToAction("Index");
            }

            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                TempData["error"] = "User ID claim is missing.";
                return RedirectToAction("Index");
            }

            var userId = userIdClaim.Value;

            var apiUrl = $"https://localhost:7232/api/User/GetSingleUser/GetSingleUser/{userId}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Failed to fetch user details.";
                return RedirectToAction("Index");
            }

            var userDetailsJson = await response.Content.ReadAsStringAsync();
            var userDetails = JsonConvert.DeserializeObject<BookingVM>(userDetailsJson);

            if (userDetails == null)
            {
                TempData["error"] = "User details not found.";
                return RedirectToAction("Index");
            }

            var bookingVm = new BookingVM
            {
                ApplicationUserId = userId,
                Name = userDetails.Name,
                Email = userDetails.Email,
                Phone = userDetails.Phone,
            };

            return View(bookingVm);
        }

        [HttpPost]

        public async Task<IActionResult> Create(BookingVM booking)
        {

            var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Create", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response Content: " + responseContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Booking created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "NO Table Available At This Time";
            return View(booking);
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
