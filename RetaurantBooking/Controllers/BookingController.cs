
using Microsoft.AspNetCore.Mvc;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System.Security.Claims;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
       
        private readonly IBookingService _bookingService;


        public BookingController( IBookingService bookingService)
        {
            
            _bookingService = bookingService;

        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] BookingVM bookingVM)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bookingVM.ApplicationUserId = userId;

            if (bookingVM == null)
            {
                return BadRequest("Invalid booking details.");
            }
            
            await _bookingService.CreateBookingAsync(bookingVM);

            return Ok(new { message = "Booking created successfully." });
        }


        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                var bookings = _bookingService.GetBookingsAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleBooking(int id)
        {
            var booking = _bookingService.GetSingle(id);
            if (booking == null)
            {
                return NotFound("Booking Not Found");
            }
            return Ok(booking);
        }

        [HttpDelete("{bookingId}")]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            var bookingToDelete = _bookingService.GetSingle(bookingId);

            if (bookingToDelete == null)
            {
                return NotFound("Booking not found.");
            }

            return Ok(bookingToDelete);
        }



        [HttpPost("{bookingId}")]
        public async  Task<IActionResult> UpdateBooking(int bookingId)
        {
            var bookingToUpdate = _bookingService.GetSingle(bookingId);

            if (bookingToUpdate == null)
            {
                return NotFound("Booking not found.");
            }

            _bookingService.Update(bookingToUpdate);

            return Ok("Booking Updated successfully.");
        }



        [HttpGet("GetBookingsByUserId/{userId}")]
        public async Task<IActionResult> GetBookingsByUserId(string userId)
        {

            var result = _bookingService.GetBookingsByUserId(userId);

            if (result == null || !result.Any())
            {
                return NotFound($"No bookings found for user with ID {userId}.");
            }

            return Ok(result);
        } 


    }
}
