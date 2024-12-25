using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
   public class BookingHeder
    {
        public int id { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]

        [ValidateNever]

        public ApplicationUser ApplicationUser { get; set; }


        public string BookingStatus { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StreetAddress { get; set; }

        [Required]

        public string City { get; set; }

        [Required]

        public string PostalCode { get; set; }


        [Required]

        public string Name { get; set; }

        public DateOnly BookingDate { get; set; }

        public TimeSpan BookingTime { get; set; }
    }
}
