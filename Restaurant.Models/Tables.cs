
using System.ComponentModel.DataAnnotations;


namespace Restaurant.Models
{
    public class Tables
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TableNumber { get; set; }
        [Required]
        public int NumberOfSeats { get; set; }
        [Required]
        public bool IsAvailable { get; set; }

    }
}
