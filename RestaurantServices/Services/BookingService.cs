using Microsoft.AspNetCore.Http;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;
using System.Security.Claims;

namespace RestaurantServices.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
      

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
   
        }

        
        public void CreateBooking(BookingVM bookingVm, string userId)
        {
            TimeOnly bookingTime;
            if (!TimeOnly.TryParse(bookingVm.BookingTime, out bookingTime))
            {
                throw new ArgumentException("Invalid booking time format.");
            }

           

            var user = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("Invalid UserId.");
            }


            var availableTables = _unitOfWork.TableRepository
                .GetAll(x => x.IsAvailable && x.NumberOfSeats >= bookingVm.NumberOfGuests)
                .OrderBy(x => x.NumberOfSeats)
                .ToList();


            if (availableTables.Count == 0)
            {
                throw new ArgumentException("No available table found for the number of guests.");
            }


            var selectedTable = availableTables.First();


            selectedTable.IsAvailable = false;
            _unitOfWork.TableRepository.UpdateTable(selectedTable);
            _unitOfWork.Save();


            var newBooking = new Booking
            {
                BookingDate = bookingVm.BookingDate,
                BookingTime = bookingTime,
                NumberOfGuests = bookingVm.NumberOfGuests,
                ApplicationUserId = userId,
                Tables = selectedTable,
            };


            _unitOfWork.BookingRepository.Add(newBooking);
            _unitOfWork.Save();

            Console.WriteLine($"Table number {selectedTable.TableNumber} has been successfully booked for {bookingVm.NumberOfGuests} guests.");
        }



        public async Task<IEnumerable<BookingVM>> GetBookingsAsync()
        {
            // Fetch bookings with related entities
            var bookings = _unitOfWork.BookingRepository
                .GetAll(
                    includeProperties: "ApplicationUser,Tables"  // Include both ApplicationUser and Tables
                )
                .ToList();

            // Map to BookingVM
            var bookingsList = bookings.Select(b => new BookingVM
            {
                BookingId = b.Id,
                BookingDate = b.BookingDate,
                BookingTime = b.BookingTime.ToString(),
                NumberOfGuests = b.NumberOfGuests,
                TableId = b.Tables.Id,
                TableNumber = b.Tables.TableNumber,
               applicationUserId = b.ApplicationUserId, // Ensure ApplicationUserId is directly mapped
                Name = b.ApplicationUser?.Name ?? "No User",
                Email = b.ApplicationUser?.Email ?? "No Email",
                Phone = b.ApplicationUser?.PhoneNumber ?? "No Phone"
            }).ToList();

            return bookingsList;
        }

    }
}
