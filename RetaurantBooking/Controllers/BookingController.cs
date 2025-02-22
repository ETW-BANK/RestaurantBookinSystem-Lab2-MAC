﻿
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
            var booking = _bookingService.GetSinle(id);
            if (booking == null)
            {
                return NotFound("Booking Not Found");
            }
            return Ok(booking);
        }

        [HttpDelete("{bookingId}")]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            var bookingToDelete = _bookingService.GetSinle(bookingId);

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
            var bookingToCancel = _bookingService.GetSinle(bookingId);

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
