using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;


namespace Restaurant.Data.Access.Repository
{
    public class BookingDetailRepository : Repository<BookingDetail>, IBookingDetailRepository
    {
        private readonly RestaurantDbContext _context;
        public BookingDetailRepository(RestaurantDbContext context):base(context) 
        {
            _context = context; 
        }
        public void Update(BookingDetail bookingDetail)
        {
            throw new NotImplementedException();
        }
    }
}
