using Restaurant.Models;


namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface IBookingDetailRepository:IRepository<BookingDetail>
    {
        void Update(BookingDetail bookingDetail);
    }
}
