using Restaurant.Models;


namespace Restaurant.Data.Access.Repository.IRepository
{
  public interface IMenuRepository:IRepository<Menue>    
    {

        void UpdateMenu(Menue menue);   
    }
}
