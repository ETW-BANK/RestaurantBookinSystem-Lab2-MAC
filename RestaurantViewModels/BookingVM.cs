using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantViewModels
{
    public class BookingVM
    {
        public int BookingId { get; set; }
        public DateOnly BookingDate { get; set; }
        public string BookingTime { get; set; }
        public int NumberOfGuests { get; set; }
        public int TableId { get; set; } // Ensure this maps to an existing table
        public string ApplicationUserId { get; set; }


    }
}


    

