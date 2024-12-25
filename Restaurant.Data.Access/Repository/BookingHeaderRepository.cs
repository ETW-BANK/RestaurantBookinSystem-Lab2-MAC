using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;


namespace Restaurant.Data.Access.Repository
{
   public class BookingHeaderRepository:Repository<BookingHeder>,IBookingHeaderRepository
    {

        private readonly RestaurantDbContext _context;
        public BookingHeaderRepository(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(BookingHeder bookingHeder)
        {
            throw new NotImplementedException();
        }
    }
}
