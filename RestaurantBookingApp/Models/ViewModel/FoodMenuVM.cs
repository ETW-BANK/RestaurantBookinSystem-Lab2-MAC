namespace RestaurantBookingApp.Models.ViewModel
{
    public class FoodMenuVM
    {
        public int Id { get; set; }


        public string Title { get; set; }

        public double price { get; set; }

        public bool IsAvailable { get; set; }


        public string? ImageUrl { get; set; }
    }
}
