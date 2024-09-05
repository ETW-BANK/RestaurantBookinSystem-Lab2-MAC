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
        public readonly RestaurantDbContext _context;

        internal DbSet<T> dbSet;

        public Repository(RestaurantDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }
        public async Task AddItemAsync(T item/*, params Expression<Func<T, object>>[] include*//*s*/)
        {
            await dbSet.AddAsync(item);
        }



        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetSingleAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            T? entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

            return entity;
        }
    

    

        public async Task RemoveAsync(int id)
        {
            T entity = await dbSet.FindAsync(id);

            dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}