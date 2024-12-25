using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class BookingDetail
    {
        public int Id { get; set; }

        [Required]

        public int BookingHeaderId { get; set; }
        [ForeignKey("BookingHeaderId")]
        [ValidateNever]
        public BookingHeder BookingHeder { get; set; }

        [Required]
        public int TableId { get; set; }
        [ForeignKey("TableId")]
        [ValidateNever]
        public Tables Tables { get; set; }
    }
}
