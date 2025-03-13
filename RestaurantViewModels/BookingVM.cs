

using Restaurant.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantViewModels
{
    public class BookingVM
    {
       
            public int BookingId { get; set; }

            [Required]
            public DateTime BookingDate { get; set; }

            [Required]
            public string BookingTime { get; set; }

            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Number of guests must be at least 1.")]
            public int NumberOfGuests { get; set; }

            [Required]
            public string ApplicationUserId { get; set; }

            [Required]
            public int TableNumber { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
    
            public string Phone { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public BookingStatus BookingStatus { get; set; }
        }
    }



    

