using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingApp.Models.ViewModel
{
    public class BookingVM
    {
        public int Id { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of guests must be at least 1.")]
        public int NumberOfGuests { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public CustomerVM? Customer { get; set; }

        [Required]
        public int TablesId { get; set; }
        public TablesVM? Tables { get; set; }
    }
}
