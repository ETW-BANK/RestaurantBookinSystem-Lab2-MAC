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
     
        public UnitOfWork(RestaurantDbContext context)
        {
            _context = context;
            TableRepository=new TableRepository(context);
            ApplicationUserRepository = new ApplicationUserRepository(context);
            BookingRepository = new BookingRepository(context); 
           
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}