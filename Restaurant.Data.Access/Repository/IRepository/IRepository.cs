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
        Task AddItemAsync(T item);

        Task <T> GetSingleAsync(int id, params Expression<Func<T, object>>[] includes);

       

        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task RemoveAsync(int id);
      
        Task SaveAsync();
    }
}
