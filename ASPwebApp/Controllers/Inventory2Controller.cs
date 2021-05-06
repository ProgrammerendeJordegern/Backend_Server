﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataBase;
using DataBase.Data;
using DataBase.Models;

namespace ASPwebApp.Controllers
{
    [Route("api/Inventory")]
    [ApiController]
    public class Inventory2Controller : ControllerBase
    {
        private readonly MyDbContext _context;
        private UnitOfWork uow;

        public Inventory2Controller(MyDbContext context)
        {
            _context = context;
            uow = new UnitOfWork(_context);
        }
        /// <summary>
        /// Get a list of all inventoryItems for a user
        /// </summary>
        /// <param name="userId">May be unneccesary in the future</param>
        /// <returns></returns>
        [HttpGet("{userid}")]
        public async Task<ActionResult<List<SimpleInventoryItem>>> GetAllInventories(int? userId)
        {
            if (userId == null) return BadRequest();
            var inventories = uow.Users.GetInventoriesWithUser((int)userId).ToList();
            var inventoryItems = new List<SimpleInventoryItem>();

            //Combine 4 list to one list
            foreach (var inventory in inventories)
            {
                foreach (var item in inventory.ItemCollection)
                {
                    var simpleItem = new SimpleInventoryItem(item);
                    inventoryItems.Add(simpleItem);
                }
            }
            return inventoryItems;
        }
        /// <summary>
        /// Get 1 inventory (fx Fridge)
        /// </summary>
        /// <param name="userId">Måske snart unødvendig</param>
        /// <param name="InventoryType">1:Freezer 2: Fridge 3: pantry 4: shopping list</param>
        /// <returns></returns>
        [HttpGet("{userId}/{InventoryType}")]
        public async Task<ActionResult<List<SimpleInventoryItem>>> Inventory(int? userId, int? InventoryType)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            Type convertedInventoryType;
            if (InventoryType == null) convertedInventoryType = typeof(Fridge);
            else
            {
                convertedInventoryType = FromEnumToType(InventoryType);
            }

            var inventory = uow.Users.GetInventoryWithUser((int)userId, convertedInventoryType);
            if (inventory == null)
            {
                return NotFound("Sorry db returned nothing");
            }
            

            //Remove unnecesary data:
            List<SimpleInventoryItem> listII = new List<SimpleInventoryItem>();
            foreach (var ii in inventory.ItemCollection)
            {
                listII.Add(new SimpleInventoryItem(ii));
            }
            return listII;
        }

        public static Type FromEnumToType(int? InventoryType)
        {
            switch ((InventoryTypes)InventoryType)
            {
                case InventoryTypes.Freezer:
                    return typeof(Freezer);
                case InventoryTypes.Fridge:
                    return typeof(Fridge);
                case InventoryTypes.Pantry:
                    return typeof(Pantry);
                case InventoryTypes.ShoppingList:
                    return typeof(ShoppingList);
                case InventoryTypes.All:
                    return typeof(ShoppingList);
                default:
                    throw new ArgumentOutOfRangeException(nameof(InventoryType), InventoryType, null);
            }
        }

        //// GET: api/Inventory2
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Inventory>>> GetInventory()
        //{
        //    return await _context.Inventory.ToListAsync();
        //}

        //// GET: api/Inventory2/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Inventory>> GetInventory(int id)
        //{
        //    var inventory = await _context.Inventory.FindAsync(id);

        //    if (inventory == null)
        //    {
        //        return NotFound();
        //    }

        //    return inventory;
        //}

        //// PUT: api/Inventory2/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutInventory(int id, Inventory inventory)
        //{
        //    if (id != inventory.InventoryId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(inventory).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!InventoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Inventory2
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Inventory>> PostInventory(Inventory inventory)
        //{
        //    _context.Inventory.Add(inventory);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetInventory", new { id = inventory.InventoryId }, inventory);
        //}

        //// DELETE: api/Inventory2/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteInventory(int id)
        //{
        //    var inventory = await _context.Inventory.FindAsync(id);
        //    if (inventory == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Inventory.Remove(inventory);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.InventoryId == id);
        }
    }
}