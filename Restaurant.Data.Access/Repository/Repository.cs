using Microsoft.EntityFrameworkCore;
using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Restaurant.Data.Access.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RestaurantDbContext _context;

        internal DbSet<T> DbSet;

        public Repository(RestaurantDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            DbSet.Add(entity);

            return entity;
        }

        public void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public async Task<T> DeleteAsync(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
            return entity;
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

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, string includeproperties = "")
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeproperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (OrderBy != null)
            {
                return OrderBy(query).ToList();
            }

            return query.ToList();
        }


        public T GetById(object id)
        {
            return DbSet.Find(id);
        }
        public async Task<T> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            DbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

        }

        public async Task<T> UpdateAsync(T entity)
        {
            DbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

    }
}