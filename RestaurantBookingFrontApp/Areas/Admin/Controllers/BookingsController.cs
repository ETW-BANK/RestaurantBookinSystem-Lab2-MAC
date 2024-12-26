using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantViewModels;
using System.Security.Claims;
using System.Text;

namespace RestaurantBookingFrontApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]
    public class BookingsController : Controller
    {
        private readonly HttpClient _httpClient;

        public BookingsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/Booking/");

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var response = await _httpClient.GetAsync("GetBookings");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<List<BookingVM>>(data);

                return Json(new { data = serviceResponse });
            }

            return Json(new { data = new List<BookingVM>(), error = "Unable to retrieve users from the server." });
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

        [HttpGet]
        public async Task<IActionResult> GetMYBookings()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "User not logged in.";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Correct the URL
                var response = await _httpClient.GetAsync($"GetBookingsByUserId/GetBookingsByUserId/{userId}");

                var data = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var bookings = JsonConvert.DeserializeObject<List<MyBookingsVM>>(data);
                    return View("Mybookings", bookings);
                }

               
                TempData["error"] = $"Unable to fetch bookings. Server returned: {response.StatusCode}. Response: {data}";
            }
            catch (Exception ex)
            {
                TempData["error"] = "An unexpected error occurred. Please try again.";
            }

            return View("Mybookings", new List<MyBookingsVM>());
        }




    }
}


