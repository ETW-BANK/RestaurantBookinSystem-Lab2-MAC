using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingApp.Models.ViewModel
{
    public class CustomerVM
    {
        [DisplayName("Customer Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]

        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]

       
        public string LasttName { get; set; }

        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]

        [DisplayName("Phone")]
        public string Phone { get; set; }
    }
}
