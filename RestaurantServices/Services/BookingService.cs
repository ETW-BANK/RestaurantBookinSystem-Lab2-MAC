using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using RestaurantServices.Services.IServices;
using RestaurantViewModels;

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
            var newBooking = new Booking
            {
                BookingDate = bookingVm.BookingDate,
                BookingTime = bookingVm.BookingTime,
                NumberOfGuests = bookingVm.NumberOfGuests,
                TableId = bookingVm.TableId,
                ApplicationUserId = userId
            };

            _unitOfWork.BookingRepository.Add(newBooking);
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<BookingVM>> GetBookingsAsync()
        {
            var bookings = _unitOfWork.BookingRepository
                .GetAll(includeProperties: "ApplicationUser,Tables") // Ensure this is correct
                .ToList();

            // Check the bookings list to see if ApplicationUser data is being included
            var bookingsList = bookings.Select(b => new BookingVM
            {
                BookingId = b.Id,
                BookingDate = b.BookingDate,
                BookingTime = b.BookingTime,
                NumberOfGuests = b.NumberOfGuests,
                TableId = b.TableId,
                Table = b.Tables,
                Userid = b.ApplicationUserId,
                Name = b.ApplicationUser?.Name ?? "No User",  // Default value for debugging
                Email = b.ApplicationUser?.Email ?? "No Email",  // Default value for debugging
                UserName = b.ApplicationUser?.PhoneNumber ?? "No Phone"  // Default value for debugging
            }).ToList();

            // Debug or log the bookingsList to check values
            return bookingsList;
        }
    }
}
