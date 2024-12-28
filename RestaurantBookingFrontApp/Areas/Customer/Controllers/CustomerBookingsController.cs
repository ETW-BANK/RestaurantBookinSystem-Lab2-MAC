using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantViewModels;
using System.Security.Claims;
using System.Text;

namespace RestaurantBookingFrontApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = StaticData.Role_Customer)]
    public class CustomerBookingsController : Controller
    {
        private readonly HttpClient _httpClient;

        public CustomerBookingsController(HttpClient httpClient)
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
        [Authorize]
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
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {

            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"UpdateBooking/{bookingId}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Booking cancelled successfully.";
                return RedirectToAction(nameof(GetMYBookings));
            }
            else
            {

                var errorDetails = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode} - {errorDetails}");


                TempData["error"] = $"Failed to cancel booking. Server responded with: {response.StatusCode}";
                return RedirectToAction(nameof(GetMYBookings));
            }
        }



        [HttpGet]
        public async Task<IActionResult> ConfirmCancelBooking(int bookingId)
        {
            var response = await _httpClient.GetAsync($"GetSingleBooking/{bookingId}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var booking = JsonConvert.DeserializeObject<BookingVM>(data);

                return View("CancelBookingConfirmation", booking);
            }

            TempData["error"] = "Booking not found.";
            return RedirectToAction(nameof(GetMYBookings));
        }

    }
}
