
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;


namespace RestaurantViewModels
{
    public class CategoryVM
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
