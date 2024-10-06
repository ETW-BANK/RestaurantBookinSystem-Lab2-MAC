using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantBookingApp.Models.ViewModel;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBookingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(HttpClient httpClient, ILogger<BookingsController> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7232/api/Booking/");
            _logger = logger;
        }

        // Index View for displaying all bookings
        [HttpGet]
        public IActionResult Index() => View();

        // Get all bookings
        [HttpGet]
        public async Task<IActionResult> GetAllBooking()
        {
            try
            {
                var response = await _httpClient.GetAsync("GetAllBookings");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<List<BookingVM>>>(data);
                    return Json(serviceResponse.Success ? new { data = serviceResponse.Data } : new { data = new List<BookingVM>(), error = serviceResponse.Message });
                }
                return Json(new { data = new List<BookingVM>(), error = "Unable to retrieve bookings from the server." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bookings.");
                return Json(new { data = new List<BookingVM>(), error = "An error occurred while fetching bookings." });
            }
        }

        // Create booking - GET
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.PageTitle = "Create Booking";
            ViewBag.ButtonLabel = "Create";
            return View(new BookingVM());
        }

        // Create booking - POST
        [HttpPost]
        public async Task<IActionResult> Create(BookingVM booking)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("CreateNewBooking", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Booking created successfully";
                    return RedirectToAction(nameof(Index));
                }

                var errorData = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error creating booking: {Error}", errorData);
                ModelState.AddModelError(string.Empty, $"Error: {errorData}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while creating booking.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
            }

            return View(booking);
        }

        // Edit booking - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"GetBooking/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var bookingResponse = JsonConvert.DeserializeObject<ServiceResponse<BookingVM>>(data);

                    if (bookingResponse?.Data != null)
                    {
                        ViewBag.PageTitle = "Edit Booking";
                        ViewBag.ButtonLabel = "Update";
                        return View("Edit", bookingResponse.Data); // Separate view for Edit
                    }
                }
                TempData["error"] = "Error fetching booking details.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching booking details for edit.");
                TempData["error"] = "An error occurred while fetching booking details.";
                return RedirectToAction("Index");
            }
        }

        // Edit booking - POST
        [HttpPost]
        public async Task<IActionResult> Edit(BookingVM booking)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("Update", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Booking updated successfully";
                    return RedirectToAction("Index");
                }

                var errorData = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error updating booking: {Error}", errorData);
                ModelState.AddModelError(string.Empty, $"Error: {errorData}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating booking.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
            }

            return View("Edit", booking);
        }

        // Delete booking - GET (Confirmation)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"GetBooking/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<BookingVM>>(data);

                    if (serviceResponse?.Success == true)
                    {
                        return View(serviceResponse.Data);
                    }
                }
                TempData["error"] = "Error fetching booking details for deletion.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching booking details for deletion.");
                TempData["error"] = "An error occurred while fetching booking details.";
                return RedirectToAction("Index");
            }
        }

        // Delete booking - POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"DeleteBooking/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Booking deleted successfully";
                }
                else
                {
                    TempData["error"] = "Error deleting booking.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting booking.");
                TempData["error"] = "An unexpected error occurred during deletion.";
            }

            return RedirectToAction("Index");
        }
    }
}
