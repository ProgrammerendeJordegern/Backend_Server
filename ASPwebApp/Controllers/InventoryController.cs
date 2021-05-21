using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataBase;
using DataBase.Data;
using DataBase.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASPwebApp.Controllers
{
    [Route("api/Inventory")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly MyDbContext _context;
        private UnitOfWork uow;

        public InventoryController(MyDbContext context)
        {
            _context = context;
            uow = new UnitOfWork(_context);
        }
        /// <summary>
        /// Get a list of all inventoryItems for a user
        /// </summary>
        /// /// <param name="Authorization">JWT token form header "Bearer 32hg4"</param>
        /// <returns></returns>
        
        [HttpGet]
        public async Task<ActionResult<List<SimpleInventoryItem>>> GetAllInventories([FromHeader] string Authorization)
        {
            int userId =await uow.UserDb.GetPpUserIdByJWT(Authorization);
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
        /// <param name="InventoryType">0:Freezer 1: Fridge 2: pantry 3: shopping list</param>
        /// <param name="Authorization">JWT token form header "Bearer 32hg4"</param>
        /// <returns></returns>
        [HttpGet("{InventoryType}")]
        public async Task<ActionResult<List<SimpleInventoryItem>>> Inventory(int InventoryType,[FromHeader] string Authorization)
        {
            int userId =await uow.UserDb.GetPpUserIdByJWT(Authorization);

            Type convertedInventoryType = FromEnumToType(InventoryType);
            

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

        /// <summary>
        /// Convert number (enum) from UI to Inventory type
        /// </summary>
        /// <param name="InventoryType">int 0-4</param>
        /// <returns>typeOf(eg. Fridge</returns>
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(InventoryType), InventoryType, null);
            }
        }

        [HttpDelete("allContent/{type}")]
        public async Task<ActionResult> DeleteAllContentInAnInventory(int type, [FromHeader] string Authorization)
        {
            var convertedType = FromEnumToType(type);
            var ids = await uow.UserDb.GetInventoryIdsByJwt(Authorization);
            Inventory inventory = null;
            foreach (var id in ids)
            {
                var tempInv=uow.Inventories.Get(id);
                if (tempInv.GetType() == convertedType) inventory = tempInv;
            }

            if (inventory == null) return NotFound("Vi kunne ikke finde et inventory");
            _context.RemoveRange(_context.InventoryItem.Where(ii=>ii.InventoryId==inventory.InventoryId));
            _context.SaveChanges();

            uow.Complete();
            return Ok("Elementerne er slettet");
        }

    }
}
