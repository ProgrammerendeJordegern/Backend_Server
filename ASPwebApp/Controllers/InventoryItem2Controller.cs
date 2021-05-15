#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataBase.Data;
using DataBase.Models;

namespace ASPwebApp.Controllers
{
    [Route("api/InventoryItem")]
    [ApiController]
    public class InventoryItem2Controller : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly UnitOfWork uow;

        public InventoryItem2Controller(MyDbContext context)
        {
            _context = context;
            uow = new UnitOfWork(_context);
        }
       
        /// <summary>
        /// get all InventoryItems containing this itemId
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        // GET: InventoryItem/get?
        [HttpGet("{ItemId}")]
        public async Task<ActionResult<List<SimpleInventoryItem>>> Get(int? itemId,[FromHeader] string Authorization)
        {
            var ids=await uow.UserDb.GetInventoryIdsByJwt(Authorization);
            if (itemId == null) return BadRequest();
            if (_context.InventoryItem.Any(ii => ii.ItemId == itemId) == false) return NotFound();
            var inIts = new List<InventoryItem>();
            foreach (var id in ids)
            {
                var inIt= await _context.InventoryItem
                    .Include(ii => ii.Item)
                    .Include(ii => ii.Inventory)
                    .Where(ii => ii.ItemId == itemId)
                    .Where(ii => ii.InventoryId ==id).FirstOrDefaultAsync();
                if(inIt!=null)inIts.Add(inIt);
            }
            
            if (inIts == null) return NotFound();
            //Copy content into simple class (JSON converter complains about too many references)
            List<SimpleInventoryItem> simpleList = new List<SimpleInventoryItem>();
            foreach (var ii in inIts)
            {
                simpleList.Add(new SimpleInventoryItem(ii) { InventoryType = ConvertTypeToEnum(ii.Inventory) });

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
                    throw new Exception("Wrong inventory type chosen");
            }
        }
        /// <summary>
        /// Edit Amount in Inventory Item
        /// </summary>
        /// <param name="inventoryItemFromClient">Json object with ItemId and InventoryId</param>
        /// <returns>202 or 400</returns>
        [HttpPut]
        public async Task<ActionResult> Edit([FromBody] InventoryItem inventoryItemFromClient)
        {
            if (inventoryItemFromClient == null) return BadRequest();
            if (inventoryItemFromClient.ItemId == null || inventoryItemFromClient.ItemId == 0)
                inventoryItemFromClient.ItemId = inventoryItemFromClient.Item.ItemId;
            InventoryItem inventoryItemFromDb = await _context.InventoryItem
                .Where(i => i.InventoryId == inventoryItemFromClient.InventoryId)
                .Where(i=> i.ItemId == inventoryItemFromClient.ItemId)
                .SingleOrDefaultAsync();
            if (inventoryItemFromDb == null)//Try again, but without date
            {
                inventoryItemFromDb=await _context.InventoryItem
                    .Where(i => i.InventoryId == inventoryItemFromClient.InventoryId)
                    .Where(i => i.ItemId == inventoryItemFromClient.ItemId)
                    .Where(i => i.DateAdded.Date == inventoryItemFromClient.DateAdded.Date)
                    .FirstOrDefaultAsync();
            } 
                
              if(inventoryItemFromDb==null)  return NotFound();
            //Manuel update database amount
            inventoryItemFromDb.Amount = inventoryItemFromClient.Amount;
            _context.Update(inventoryItemFromDb);
            await _context.SaveChangesAsync();
            return Accepted();
        }

        /// <summary>
        /// Create a new inventoryItem, but NOT a new Item
        /// </summary>
        /// <param name="userId">Forsvinder snart</param>
        /// <param name="type">0: freezer 1:Fridge 2:Pantry 3: Shopping</param>
        /// <param name="inventoryItem"></param>
        /// <param name="Authorization">JWT token form header "Bearer 32hg4"</param>
        /// <returns></returns>
        [HttpPost("existingItem/{userId}/{type}")]
        [HttpPost("existingItem/{type}")]
        public async Task<ActionResult> CreateWExistingItem(int? userId, [Required]int? type, [FromBody] SimpleInventoryItem? inventoryItem,[FromHeader]string Authorization)
        {
            if (inventoryItem == null) return NoContent();
            Inventory inventory;
            if (userId == null) userId =await uow.UserDb.GetPpUserIdByJWT(Authorization);
            if (type != null)//Type supplied as parameter
            {
                var uow = new UnitOfWork(_context);
                inventory = uow.Users.GetInventoryWithUser((int)userId, PpUserController.FromEnumToType(type));
            }
            else//Type is embedded in json object
            {
                inventory = await _context.Inventory.SingleAsync(i => i.InventoryId == inventoryItem.InventoryId);
            }
            if (inventory == null) return BadRequest();
            var item = await _context.Item.SingleAsync(i => i.ItemId == inventoryItem.ItemId);
            if (item == null) return BadRequest();

            //If mathcing item allready is added today - increment the existing:
            var IIattempt=await uow.InventoryItems.TryGetTodayIinventoryItem(inventory.InventoryId, item.ItemId);
            if (IIattempt != null)
            {
                IIattempt.Amount += inventoryItem.Amount;
                uow.Complete();
                return Ok("InventoryItem allerede oprettet i dag. den er opdateret");
            }
            //Else: create new inventory item:
            var completeInventoryItem = new InventoryItem(inventoryItem);
            completeInventoryItem.Item = item;
            completeInventoryItem.ItemId = item.ItemId;
            completeInventoryItem.Inventory = inventory;
            completeInventoryItem.InventoryId = inventory.InventoryId;
            completeInventoryItem.DateAdded = DateTime.Now;
            _context.Add(completeInventoryItem);
            int result = await _context.SaveChangesAsync();
            if (result > 0) return Accepted();
            return BadRequest();

        }

        /// <summary>
        /// Create a new InventoryItem AND a new Item
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type">0-3</param>
        /// <param name="inventoryItem"></param>
        /// <param name="Authorization">JWT token form header "Bearer 32hg4"</param>
        /// <returns></returns>
        [HttpPost("newItem/{userId}/{type}")]
        [HttpPost("newItem/{type}")]
        public async Task<ActionResult> CreateWNewItem(int? userId, int? type, [FromBody] InventoryItem? inventoryItem,[FromHeader]string Authorization)
        {
            if (inventoryItem == null) return NoContent();
            Inventory? inventory = null;
            if (userId == null) userId = await uow.UserDb.GetPpUserIdByJWT(Authorization);
            if (type != null)
            {
                inventory = uow.Users.GetInventoryWithUser((int)userId, PpUserController.FromEnumToType(type));
            }
            //else inventory = await _context.Inventory.SingleAsync(i => i.InventoryId == inventoryItem.InventoryId);
            if (inventory == null) return BadRequest();
            inventoryItem.Inventory = inventory;
            inventoryItem.DateAdded = DateTime.Now;
            _context.Add(inventoryItem);
            int result = await _context.SaveChangesAsync();
            if (result > 0) return Accepted();
            return BadRequest();

        }

        /// <summary>
        /// Delete an inventory item (eg. when amount reached 0
        /// </summary>
        /// <param name="itemId">itemId for the item</param>
        /// <param name="dateTime">date to destinguish between II (time-part is ignored)</param>
        /// <param name="Authorization">JWT token form header "Bearer 32hg4"</param>
        /// <returns>202 accepted</returns>
        // DELETE: api/InventoryItem2/5
        [HttpDelete("{itemId}/{dateTime}")]
        public async Task<IActionResult> DeleteInventoryItem(int itemId,DateTime dateTime,[FromHeader]string Authorization)
        {
            var inventoryItemIds=await uow.UserDb.GetInventoryIdsByJwt(Authorization);
            var succeeded = await uow.InventoryItems.Delete(itemId, dateTime, inventoryItemIds);
            if (!succeeded)
            {
                return NotFound("Vi kunne ikke finde elementet i databasen");
            }
            
            return Accepted();
        }

        private bool InventoryItemExists(int id)
        {
            return _context.InventoryItem.Any(e => e.InventoryId == id);
        }
    }
}
