

using Restaurant.Models;

namespace RestaurantViewModels
{
    public class MyBookingsVM
    {

        public int BookingId { get; set; }
        public DateOnly BookingDate { get; set; }
        public string BookingTime { get; set; }
        public int NumberOfGuests { get; set; }
        public int TableId { get; set; }
        public string ApplicationUserId { get; set; }
        public int TableNumber { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public BookingStatus BookingStatus { get; set; }
    }
}
