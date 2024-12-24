using Restaurant.Models;


namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface IBookingRepository:IRepository<Booking>
    {

        void UpdateBooking (Booking booking);   
    }
}
