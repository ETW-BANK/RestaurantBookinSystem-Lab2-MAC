using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingApp.Models.ViewModel
{
    public class TablesVM
    {
        public int Id { get; set; }

        [Required]

        [DisplayName("Table Number")]
        public int TableNumber { get; set; }

        [Required]

        [DisplayName("Number Of Seats")]
        public int NumberOfSeats { get; set; }

        [Required]

        [DisplayName("isAvailable")]
        public bool isAvialable { get; set; }
    }
}
