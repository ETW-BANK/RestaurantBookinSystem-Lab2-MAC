using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly RestaurantDbContext _context;

        public UnitOfWork(RestaurantDbContext context)
        {
            _context = context;
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                this.disposed = true;
            }
        }

        public IRepository<T> Repository<T>() where T : class
        {
            IRepository<T> repo = new Repository<T>(_context);
            return repo;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}