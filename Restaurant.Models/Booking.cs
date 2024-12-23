
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace Restaurant.Models
{
   public class Booking
    {
        [Key]
        public int Id { get; set; } // Primary key for Booking

        [Required]
        public DateOnly BookingDate { get; set; }

        [Required]
        public TimeSpan BookingTime { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Number of guests must be between 1 and 20.")]
        public int NumberOfGuests { get; set; }

        // Foreign key for the related table
        [Required]
        public int TableId { get; set; }

        // Navigation property for the related table
        [ForeignKey(nameof(TableId))]
        public Tables Tables { get; set; }

        // Foreign key for the user who created the booking
        [Required]
        public string ApplicationUserId { get; set; }

        // Navigation property for the related user
        public ApplicationUser ApplicationUser { get; set; }
    }
}
