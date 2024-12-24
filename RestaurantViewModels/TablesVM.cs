
using System.ComponentModel.DataAnnotations;


namespace RestaurantViewModels
{
    public class TablesVM
    {

        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int TableNumber { get; set; }
        [Required]
        public int NumberOfSeats { get; set; }
        public bool IsAvailable { get; set; }
    }
}
