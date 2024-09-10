using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingApp.Models.ViewModel
{
    public class MenuVM
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Food Name")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Price")]
        public double price { get; set; }

        [Required]
        [DisplayName("Is Available")]
        public bool IsAvailable { get; set; }

      
        [DisplayName("Food Image")]

        public string? ImageUrl { get; set; }
    }
}
