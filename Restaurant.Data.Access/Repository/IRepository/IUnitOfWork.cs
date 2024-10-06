using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;

        void Save();
    }
}
