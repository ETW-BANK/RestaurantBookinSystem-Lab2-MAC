using Restaurant.Models;
using RestaurantViewModels;


namespace RestaurantServices.Services.IServices
{
    public interface IBookingService
    {
        void CreateBooking(BookingVM booking, string userId);
    }
}
