using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;


namespace Restaurant.Data.Access.Repository
{
    public class BookingRepository:Repository<Booking>,IBookingRepository
    {
        private readonly RestaurantDbContext _db;

        public BookingRepository(RestaurantDbContext db):base(db) 
        {
            _db = db;
        }

        public void UpdateBooking(Booking booking)
        {
           _db.Bookings.Update(booking);
        }
    }
}
