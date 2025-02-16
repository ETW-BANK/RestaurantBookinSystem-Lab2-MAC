using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;


namespace Restaurant.Data.Access.Repository
{
    public class CategoryRepository: Repository<Category>,ICategoryRepository
    {

        private readonly RestaurantDbContext _db;

        public CategoryRepository(RestaurantDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            _db.Update(category);
        }
    }
}
