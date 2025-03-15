using Microsoft.AspNetCore.Http;
using Restaurant.Models;
using RestaurantViewModels;


namespace RestaurantServices.Services.IServices
{
    public interface IBookingService
    {
        Task CreateBookingAsync(BookingVM bookingVM);

        Task <IEnumerable<BookingVM>> GetBookingsAsync();

        Task<Booking> DeleteBooking(Booking booking);

        BookingVM GetSingle(int id);
        IEnumerable<MyBookingsVM> GetBookingsByUserId(string userId);
        Booking CancelBooking(Booking booking);
        void Update(BookingVM booking);
    }
}
