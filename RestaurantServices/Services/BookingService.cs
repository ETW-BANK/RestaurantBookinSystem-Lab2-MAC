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
                ApplicationUserId = userId,
                TableId = bookingVm.TableId
            };

            _unitOfWork.BookingRepository.Add(newBooking);
            _unitOfWork.Save();
        }

    }
}
