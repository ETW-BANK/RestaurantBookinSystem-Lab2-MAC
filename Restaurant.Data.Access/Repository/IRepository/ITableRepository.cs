using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface ITableRepository: IRepository<Tables>
    {
        void UpdateTable(Tables tables);
    }
}
