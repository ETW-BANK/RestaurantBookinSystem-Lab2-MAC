using Restaurant.Models;
using RestaurantViewModels;


namespace RestaurantServices.Services.IServices
{
    public interface IBookingService
    {
        void CreateBooking(BookingVM booking);

        Task<IEnumerable<BookingVM>> GetBookingsAsync();

        Booking DeleteBooking(Booking booking);

        Booking GetSinle(int bookingId);
        IEnumerable<MyBookingsVM> GetBookingsByUserId(string userId);
    }
}
