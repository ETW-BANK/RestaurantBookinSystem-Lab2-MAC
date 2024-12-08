using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ITableRepository TableRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
      IBookingRepository BookingRepository { get; }

        
        void Save();
    }
}
