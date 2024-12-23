using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Models;
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
        public async Task<IActionResult> Create()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Unable to fetch user details. Please log in again.";
                return RedirectToAction("Index");
            }


            var apiUrl = $"https://localhost:7232/api/User/GetUserDetails/details/{userId}";

            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                TempData["error"] = "Failed to fetch user details.";
                return RedirectToAction("Index");
            }

            var userDetailsJson = await response.Content.ReadAsStringAsync();
            var userDetails = JsonConvert.DeserializeObject<UserDetailsVM>(userDetailsJson);

            if (userDetails == null)
            {
                TempData["error"] = "User details not found.";
                return RedirectToAction("Index");
            }

            //var bookingVM = new BookingVM
            //{
            //    applicationUserId = userId,
            //    Name = userDetails.Name,
            //    Phone = userDetails.Phone,
            //    Email = userDetails.Email
            //};

            return View(/*bookingVM*/);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingVM booking)
        {
            // Log the booking details (payload)
            var bookingJson = JsonConvert.SerializeObject(booking);
            Console.WriteLine("Sending Booking Payload: " + bookingJson);

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid booking details.";
                return View(booking);
            }

            // Retrieve user ID from claims
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Unable to fetch user details. Please log in again.";
                return RedirectToAction(nameof(Index));
            }


            //booking.applicationUserId = userId;

            var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");


            var response = await _httpClient.PostAsync("Create", content);


            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response Content: " + responseContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Booking created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to create booking. Please try again.";
            return View(booking);
        }
    }
}


