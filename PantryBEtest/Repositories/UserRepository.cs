using System.Collections.Generic;
using System.Linq;
using DataBase.Data;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories
{
    public interface IUserRepository : IRepository<PpUser>
    {
        ICollection<Inventory> GetInventoriesWithUser(int id);
    }
    public class UserRepository : Repository<PpUser>, IUserRepository
    {
        public UserRepository(MyDbContext context) : base(context)
        {
        }

        public MyDbContext PlutoContext
        {
            get { return Context as MyDbContext; }
        }

        public ICollection<Inventory> GetInventoriesWithUser(int id)
        {
            List<PpUser> user = PlutoContext.PpUser
                .Where(u => u.PpUserId.Equals(id))
                .Include(u => u.Inventories)
                .ThenInclude(i => i.ItemCollection)
                .ThenInclude(i=>i.Item)
                .ToList();
            return user[0].Inventories;
        }
    }
}
