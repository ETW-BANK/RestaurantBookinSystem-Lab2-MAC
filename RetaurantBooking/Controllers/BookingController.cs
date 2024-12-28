
using Microsoft.AspNetCore.Mvc;
using Restaurant.Data.Access.Repository.IRepository;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System.Net.Http;
using System.Security.Claims;

namespace RetaurantBooking.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingService _bookingService;


        public BookingController(IUnitOfWork unitOfWork, IBookingService bookingService)
        {
            _unitOfWork = unitOfWork;
            _bookingService = bookingService;

        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] BookingVM bookingVM)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (bookingVM == null)
            {
                return BadRequest("Invalid booking details.");
            }

            _bookingService.CreateBooking(bookingVM);

            return Ok(new { message = "Booking created successfully." });
        }


        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            try
            {
                var bookings = await _bookingService.GetBookingsAsync();
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
            var booking = _unitOfWork.BookingRepository.GetFirstOrDefault(x => x.Id == id);
            if (booking == null)
            {
                return NotFound("Booking Not Found");
            }
            return Ok(booking);
        }

        [HttpDelete("{bookingId}")]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            var bookingToDelete = _unitOfWork.BookingRepository.GetFirstOrDefault(x => x.Id == bookingId);

            if (bookingToDelete == null)
            {
                return NotFound("Booking not found.");
            }


            _bookingService.DeleteBooking(bookingToDelete);

            return Ok("Booking deleted successfully.");
        }

        [HttpPost("{bookingId}")]
        public async Task<IActionResult> UpdateBooking(int bookingId)
        {
            var bookingToCancel = _unitOfWork.BookingRepository.GetFirstOrDefault(x => x.Id == bookingId);

            if (bookingToCancel == null)
            {
                return NotFound("Booking not found.");
            }

            _bookingService.CancelBooking(bookingToCancel);

            return Ok("Booking Cancelled successfully.");
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
