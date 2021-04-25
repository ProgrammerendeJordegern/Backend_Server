using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataBase.Data;
using DataBase.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace ASPwebApp.Controllers
{
    public class InventoryItemController : Controller
    {
        private readonly MyDbContext _context;

        public InventoryItemController(MyDbContext context)
        {
            _context = context;
        }


        // GET: InventoryItem/get?
        public async Task<ActionResult<List<SimpleInventoryItem>>> Get(int? ItemId)
        {
            if (ItemId == null) return BadRequest();
            if (_context.InventoryItem.Any(ii => ii.ItemId == ItemId)==false) return NotFound();
            var inIt = await _context.InventoryItem
                .Include(ii => ii.Item)
                .Include(ii=>ii.Inventory)
                .Where(ii => ii.ItemId == ItemId).ToListAsync();
            if (inIt == null) return NotFound();
            //Copy content into simple class (JSON converter complains about too many references)
            List<SimpleInventoryItem> simpleList = new List<SimpleInventoryItem>();
            foreach (var ii in inIt)
            {
                simpleList.Add( new SimpleInventoryItem(ii){InventoryType = ConvertTypeToEnum(ii.Inventory)});
                
            }
            return simpleList;
        }

        private InventoryTypes ConvertTypeToEnum(Inventory inventory)
        {
            switch (inventory)
            {
                case Freezer freezer:
                    return InventoryTypes.Freezer;
                case Fridge fridge:
                    return InventoryTypes.Fridge;
                case Pantry pantry:
                    return InventoryTypes.Pantry;
                case ShoppingList shoppingList:
                    return InventoryTypes.ShoppingList;
               default:
                    return InventoryTypes.All;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] InventoryItem? inventoryItemFromClient)
        {
            if (inventoryItemFromClient == null) return BadRequest();
            InventoryItem inventoryItemFromDb = await _context.InventoryItem
                .Include(i => i.Item)
                .SingleAsync(i => i.InventoryId == inventoryItemFromClient.InventoryId && i.ItemId == inventoryItemFromClient.ItemId);
            if (inventoryItemFromDb == null) return NotFound();
            //Manuel update database entry (only wanted changes are copied)
            inventoryItemFromDb.Amount = inventoryItemFromClient.Amount;
            inventoryItemFromDb.Item.Name = inventoryItemFromClient.Item.Name;
            inventoryItemFromDb.Item.AverageLifespanDays = inventoryItemFromClient.Item.AverageLifespanDays;
            inventoryItemFromDb.Item.DesiredMinimumAmount = inventoryItemFromClient.Item.DesiredMinimumAmount;
            inventoryItemFromDb.Item.Ean = inventoryItemFromClient.Item.Ean;
            inventoryItemFromDb.Item.Size = inventoryItemFromClient.Item.Size;
            _context.Update(inventoryItemFromDb);
            await _context.SaveChangesAsync();
            return Accepted();
        }


        public async Task<IActionResult> CreateWExistingItem(int? userId,int? type,[FromBody] SimpleInventoryItem? inventoryItem)
        {
            if (inventoryItem == null) return NoContent();
            Inventory inventory;
            if (type != null&&userId!=null)
            {
                var uow = new UnitOfWork(_context);
                inventory = uow.Users.GetInventoryWithUser((int)userId, PpUserController.FromEnumToType(type));
                   
            }
            else 
            { 
                inventory = await _context.Inventory.SingleAsync(i => i.InventoryId == inventoryItem.InventoryId);
            }
            if (inventory == null) return BadRequest();
            var item =await  _context.Item.SingleAsync(i => i.ItemId == inventoryItem.ItemId);
            if (item == null) return BadRequest();
            var completeInventoryItem = new InventoryItem(inventoryItem);
            completeInventoryItem.Item = item;
            completeInventoryItem.ItemId = item.ItemId;
            completeInventoryItem.Inventory = inventory;
            completeInventoryItem.InventoryId = inventory.InventoryId;
            completeInventoryItem.DateAdded=DateTime.Now;
            _context.Add(completeInventoryItem);
           int result= await _context.SaveChangesAsync();
           if (result > 0) return Accepted();
           return BadRequest();

        }
        public async Task<IActionResult> CreateWNewItem(int? userId, int? type, [FromBody] InventoryItem? inventoryItem)
        {
            if (inventoryItem == null) return NoContent();
            Inventory inventory;
            if (userId != null && type!=null)
            {
                var uow = new UnitOfWork(_context);
                inventory= uow.Users.GetInventoryWithUser((int) userId, PpUserController.FromEnumToType(type));
            }
            else inventory = await _context.Inventory.SingleAsync(i => i.InventoryId == inventoryItem.InventoryId);
            if (inventory == null) return BadRequest();
            inventoryItem.Inventory = inventory;
            inventoryItem.DateAdded = DateTime.Now;
            _context.Add(inventoryItem);
            int result = await _context.SaveChangesAsync();
            if (result > 0) return Accepted();
            return BadRequest();

        }

        private bool InventoryItemExists(int id)
        {
            return _context.InventoryItem.Any(e => e.InventoryId == id);
        }
    }
}
