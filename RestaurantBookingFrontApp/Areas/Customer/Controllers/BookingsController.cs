using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Services;
using Restaurant.Utility;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System.Security.Claims;

namespace RestaurantBookingFrontApp.Areas.Customer
{
    [Area("Customer")]
    [Authorize(Roles = StaticData.Role_Customer)]
    public class BookingsController : Controller
    {

        private readonly IBookingService _bookingService;
        private readonly ICategoryService _categoryService;
        public BookingsController(IBookingService bookingService,ICategoryService categoryService)
        {
            _bookingService = bookingService;
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
            public IActionResult Upsert(int id)
        {
            BookingVM bookingVM = new();

            if (id == 0)
            {
                return View(bookingVM);
            }
            else
            {
                var bookings = _bookingService.GetSingle(id);
                if (bookings == null) 
                {
                    return NotFound();
                }

                return View(bookings);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upsert(BookingVM bookingVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bookingVM.ApplicationUserId = userId;

            try
            {
                if (bookingVM.BookingId == 0)
                {
                    // Create mode
                    await _bookingService.CreateBookingAsync(bookingVM);
                    TempData["success"] = "Booking created successfully.";
                }
                else
                {
                    // Update mode
                    _bookingService.Update(bookingVM); // Await the Update method
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

    }
}
