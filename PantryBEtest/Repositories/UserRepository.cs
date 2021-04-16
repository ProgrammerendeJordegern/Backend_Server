using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Data;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories
{
    public interface IUserRepository : IRepository<PpUser>
    {
        public ICollection<Inventory> GetInventoriesWithUser(int id);
        public Inventory GetInventoryWithUser(int id, Type type);
        public PpUser GetUserWithEmail(string email);
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
            if (user.Count>0) return user[0].Inventories;
            return null;
        }

        public Inventory GetInventoryWithUser(int id, Type type)
        {
            //find user
            PpUser user = PlutoContext.PpUser
                .Include(u => u.Inventories)
                .Single(u => u.PpUserId.Equals(id));
            //find inventory
            if (!user.Inventories.Any()) return null;
            var inventoryId = user?.Inventories?.Single(i => i?.GetType() == type)?.InventoryId;
           //get items in inventory
           if (inventoryId == null) return null;
            var inventory= PlutoContext.Inventory.Include(i=>i.ItemCollection)
                .ThenInclude(ic=>ic.Item)
                .Single(i => i.InventoryId == inventoryId);
            return inventory;
        }

        public PpUser GetUserWithEmail(string email)
        {
            PpUser user = PlutoContext.PpUser
                .SingleOrDefault(u => u.Email.Equals(email));
            return user;
        }
    }
}
