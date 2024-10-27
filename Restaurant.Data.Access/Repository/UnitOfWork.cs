using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly RestaurantDbContext _context;

        public IApplicationUserRepository ApplicationUserRepository { get; private set; }
        public ITableRepository TableRepository { get; private set; }
        public UnitOfWork(RestaurantDbContext context)
        {
            _context = context;
            TableRepository=new TableRepository(context);
            ApplicationUserRepository = new ApplicationUserRepository(context);
        }

      

       

       

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}