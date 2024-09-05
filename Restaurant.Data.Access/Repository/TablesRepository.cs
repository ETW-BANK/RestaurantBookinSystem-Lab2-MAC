using Restaurant.Data.Access.Data;
using Restaurant.Data.Access.Repository.IRepository;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Data.Access.Repository
{
    public class TablesRepository : Repository<Tables>, ITableRepository
    {
        private readonly RestaurantDbContext _context;
        public TablesRepository(RestaurantDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateTableAsync(Tables table)
        {
            var existingTable = await GetSingleAsync(table.Id);

            if (existingTable != null)
            {
                _context.Table.Update(existingTable);
            }
            await _context.SaveChangesAsync();
        }
    }
}
