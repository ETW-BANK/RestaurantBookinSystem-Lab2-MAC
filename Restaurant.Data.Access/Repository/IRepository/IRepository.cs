using System.Linq.Expressions;

namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);

        void Add(T entity);

        void Remove(T item);

        void RemoveRange(IEnumerable<T> items);
    }
}
