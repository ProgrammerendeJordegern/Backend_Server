using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataBase.Data;
using DataBase.Models;

namespace ASPwebApp.Controllers
{
    public class InventoryItemController : Controller
    {
        private readonly MyDbContext _context;

        public InventoryItemController(MyDbContext context)
        {
            _context = context;
        }


        // GET: InventoryItem/Details/5
        public async Task<ActionResult<SimpleInventoryItem>> Get(int? inventoryItemId)
        {
            if (inventoryItemId == null) return BadRequest();
            InventoryItem inIt = await _context.InventoryItem
                .Include(i => i.Item)
                .SingleAsync(i => i.InventoryId == inventoryItemId);
            if (inIt == null) return NotFound();
            //Copy content into simple class (JSON converter complains about too many references)
            var simpelInIt = new SimpleInventoryItem(inIt);
            return simpelInIt;
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


        public async Task<IActionResult> CreateWExistingItem([FromBody] InventoryItem? inventoryItem)
        {
            if (inventoryItem == null) return NoContent();
            var inventory = await _context.Inventory.SingleAsync(i => i.InventoryId == inventoryItem.InventoryId);
            if (inventory == null) return BadRequest();
            var item =await  _context.Item.SingleAsync(i => i.ItemId == inventoryItem.ItemId);
            if (item == null) return BadRequest();
            inventoryItem.Inventory = inventory;
            inventoryItem.Item = item;
            inventoryItem.DateAdded=DateTime.Now;
            _context.Add(inventoryItem);
           int result= await _context.SaveChangesAsync();
           if (result > 0) return Accepted();
           return BadRequest();

        }
        public async Task<IActionResult> CreateWNewItem([FromBody] InventoryItem? inventoryItem)
        {
            if (inventoryItem == null) return NoContent();
            var inventory = await _context.Inventory.SingleAsync(i => i.InventoryId == inventoryItem.InventoryId);
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
