using Restaurant.Models;


namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface IBookingHeaderRepository:IRepository<BookingHeder>
    {
        void Update(BookingHeder bookingHeder); 
    }
}
