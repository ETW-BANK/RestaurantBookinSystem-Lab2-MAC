
using System.ComponentModel.DataAnnotations;


namespace RestaurantViewModels
{
   public class UserVm
    {
        [Required]
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }




    }
}
