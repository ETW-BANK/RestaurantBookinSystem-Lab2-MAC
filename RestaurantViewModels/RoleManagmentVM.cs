using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Models;


namespace RestaurantViewModels
{
    public class RoleManagmentVM
    {
        public ApplicationUser ApplicationUser { get; set; }    
        public IEnumerable<SelectListItem> RoleList { get; set; }

       

    }
}
