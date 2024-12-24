
using Restaurant.Models;


namespace Restaurant.Data.Access.Repository.IRepository
{
    public interface ITableRepository: IRepository<Tables>
    {
        void UpdateTable(Tables tables);
    }
}
