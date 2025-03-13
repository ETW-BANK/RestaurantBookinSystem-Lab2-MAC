using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Restaurant.Utility;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System.Security.Claims;
using System.Text;

namespace RestaurantBookingFrontApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)] 
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService; 
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var Bookinglist = await _bookingService.GetBookingsAsync();

            if (Bookinglist == null)
            {

                TempData["error"] = "An error occurred while retrieving bookings.";
            }

            return View(Bookinglist.ToList());
        }


        public IActionResult Upsert(int id)
        {
           BookingVM booking = new();

            if (id == 0)
            {

                return View(booking);
            }
            else
            {

                var bookings= _bookingService.GetSinle(id); 
                if (bookings== null)
                {
                    return NotFound();
                }


                return View(booking);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(BookingVM bookingVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bookingVM.ApplicationUserId = userId; 

            try
            {
                if (bookingVM.BookingId == 0)
                {
                 
                   await _bookingService.CreateBookingAsync(bookingVM);
                    TempData["success"] = "Booking created successfully.";
                }
                else
                {
                
                    TempData["success"] = "Booking updated successfully.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred: {ex.Message}";
                return View(bookingVM);
            }
        }





      



        //[HttpPost]
        //public async Task<IActionResult> CancelBooking(int bookingId)
        //{

        //    var content = new StringContent("{}", Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync($"UpdateBooking/{bookingId}", content);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        TempData["success"] = "Booking cancelled successfully.";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {

        //        var errorDetails = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine($"Error: {response.StatusCode} - {errorDetails}");


        //        TempData["error"] = $"Failed to cancel booking. Server responded with: {response.StatusCode}";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}




        //[HttpGet]
        //public async Task<IActionResult> ConfirmCancelBooking(int bookingId)
        //{
        //    var response = await _httpClient.GetAsync($"GetSingleBooking/{bookingId}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = await response.Content.ReadAsStringAsync();
        //        var booking = JsonConvert.DeserializeObject<BookingVM>(data);

        //        return View("CancelBookingConfirmation", booking);
        //    }

        //    TempData["error"] = "Booking not found.";
        //    return RedirectToAction(nameof(Index));
        //}

    }
}
