
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Models
{
   public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        [Range(1, 100)]
        public int DisplayOrder { get; set; }

        [ValidateNever]
        public string? ImageUrl { get; set; }    

    }
}
