using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Data.Access.Repository.Services;
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
            var existingBooking = await _context.Bookings.FirstOrDefaultAsync(u => u.Id == booking.Id);
            if (existingBooking != null)
            {
                _context.Entry(existingBooking).CurrentValues.SetValues(booking);
                await _context.SaveChangesAsync();
            }
        }




    }
}
