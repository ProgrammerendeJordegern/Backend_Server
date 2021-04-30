using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase.Data;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        public Task<Item> GetItemWithEan(string ean);
        
    }


    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(MyDbContext context) : base(context)
        {
        }

        public MyDbContext PlutoContext
        {
            get { return Context as MyDbContext; }
        }


        public async Task<Item> GetItemWithEan(string ean)
        {
           return await PlutoContext.Item.SingleOrDefaultAsync(i=>i.Ean==ean);
        }
    }
}
