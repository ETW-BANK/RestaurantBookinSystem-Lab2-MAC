using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;

namespace Restaurant.Data.Access.Repository
{
    public class TableRepository: Repository<Tables>,ITableRepository
    {

        private readonly RestaurantDbContext _db;

        public TableRepository(RestaurantDbContext db) : base(db)
        {
            _db = db;
        }

        public void UpdateTable(Tables tables)
        {
            _db.Table.Update(tables);
        }
    }
}
