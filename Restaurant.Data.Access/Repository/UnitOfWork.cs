using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;

namespace Restaurant.Data.Access.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly RestaurantDbContext _context;
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }
        public ITableRepository TableRepository { get; private set; }
        public IBookingRepository BookingRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public IMenuRepository MenuRepository { get; private set; }
        public UnitOfWork(RestaurantDbContext context)
        {
            _context = context;
            TableRepository=new TableRepository(context);
            ApplicationUserRepository = new ApplicationUserRepository(context);
            BookingRepository = new BookingRepository(context);
            MenuRepository = new MenuRepository(context);
            CategoryRepository = new CategoryRepository(context);   



        }

        public async Task SaveAsync()
        {
        _context.SaveChanges();
        }

    }
}