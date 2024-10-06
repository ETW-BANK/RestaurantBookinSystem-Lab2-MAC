using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, string includeproperties = "");

        T GetById(object id);
        Task<T> GetByIdAsync(object id);

        void Add(T entity);
        Task<T> AddAsync(T entity);

        void Update(T entity);
        Task<T> UpdateAsync(T entity);

        void Delete(T entity);
        Task<T> DeleteAsync(T entity);
    }
}
