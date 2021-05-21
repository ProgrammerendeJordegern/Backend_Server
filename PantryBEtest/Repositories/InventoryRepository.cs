using DataBase.Data;

namespace DataBase.Repositories
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
       
    }


    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(MyDbContext context) : base(context)
        {
        }

        
    }
}
