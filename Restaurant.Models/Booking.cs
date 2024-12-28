
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
   public class Booking
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public DateOnly BookingDate { get; set; }

        [Required]
        public TimeSpan BookingTime { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Number of guests must be between 1 and 20.")]
        public int NumberOfGuests { get; set; }
 
        [Required]
        public int TableId { get; set; }
        
        [ForeignKey(nameof(TableId))]
        public Tables Tables { get; set; }
       
        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public BookingStatus BookingStatus { get; set; }    
    }

    public enum BookingStatus
    {
        Active,
        Cancelled,
    }
}
