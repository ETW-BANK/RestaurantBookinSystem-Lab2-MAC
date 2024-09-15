using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository
{
    public class BookingRepository : Repository<Booking>, IBookngRepository
    {
        private readonly RestaurantDbContext _context;

        public BookingRepository(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task UpdateBookingAsync(Booking booking)
        {
            var existingBooking = await _context.Bookings
                .Include(b => b.Tables)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(u => u.Id == booking.Id);

            if (existingBooking != null)
            {
                // Update properties of the existing booking
                existingBooking.NumberOfGuests = booking.NumberOfGuests;
                existingBooking.BookingDate = booking.BookingDate;
                existingBooking.TablesId = booking.TablesId;
                existingBooking.CustomerId = booking.CustomerId;

                // Optionally set navigation properties (if needed)
                existingBooking.Tables = booking.Tables;
                existingBooking.Customer = booking.Customer;

                _context.Bookings.Update(existingBooking);
            }

            await _context.SaveChangesAsync();
        }




    }
}
