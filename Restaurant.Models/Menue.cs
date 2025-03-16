using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Restaurant.Models
{
    public class Menue
    {
        [Key]
        public int menueId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? Qty { get; set; }

        [ValidateNever]
        public string? ImageUrl { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }  
        public Available? Available { get; set; }

    }

    public enum Available
    {
         NO,
         Yes
    }
}
