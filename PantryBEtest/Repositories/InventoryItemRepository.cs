using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase.Data;
using DataBase.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories
{
    public interface IInventoryItemRepository : IRepository<InventoryItem>
    {
        public Task<InventoryItem> Get(int itemId, DateTime date, int[] inventoryItemIds);
        public Task<bool> Delete(int itemId, DateTime date,int[] inventoryItemIds);
    }


    public class InventoryItemRepository : Repository<InventoryItem>, IInventoryItemRepository
    {
        public InventoryItemRepository(MyDbContext context) : base(context)
        {
        }

        public MyDbContext PlutoContext
        {
            get { return Context as MyDbContext; }
        }


        public async Task<InventoryItem> Get(int itemId, DateTime date, int[] inventoryItemIds)
        {
            var inventories = PlutoContext.InventoryItem
                .Where(i => i.ItemId == itemId)
                .Where(i => i.DateAdded.Date == date.Date);
            InventoryItem? theInventory=null;
            foreach (var id in inventoryItemIds)
            {
                foreach (var i in inventories)
                {
                    if (i.InventoryId == id)
                    {
                        theInventory =  i;
                    }
                }

            }

            return theInventory;
        }

        public async Task<bool> Delete(int itemId, DateTime date,int[] inventoryItemIds)
        {
            var inventoryItem = await Get(itemId, date, inventoryItemIds);
            if (inventoryItem == null)
            {
                return false;
            }

            PlutoContext.InventoryItem.Remove(inventoryItem);
            await PlutoContext.SaveChangesAsync();

            return true;
        }
    }
}
