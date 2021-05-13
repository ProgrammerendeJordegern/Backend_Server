using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataBase;
using DataBase.Data;
using DataBase.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace ASPwebApp.Controllers
{
    public class PpUserController : Controller
    {
        private readonly MyDbContext _context;
        private UnitOfWork uow;

        public PpUserController(MyDbContext context)
        {
            _context = context;
            uow = new UnitOfWork(context);
        }



        // GET: PpUser
        public async Task<IActionResult> Index()
        {

            return View(await _context.PpUser.ToListAsync());
        }

        // GET: PpUser/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var ppUser = await _context.PpUser
        //        .FirstOrDefaultAsync(m => m.PpUserId == id);
        //    if (ppUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(ppUser);
        //}
        // GET: PpUser/Details/5
        //public async Task<IActionResult> Details(string? email)
        //{
        //    if (email == null)
        //    {
        //        return NotFound();
        //    }

        //    var ppUser = uow.Users.GetUserWithEmail(email);
        //        if (ppUser == null)
        //        {
        //            return NotFound();
        //}

        //    return View(ppUser);
        //}

        public async Task<ActionResult<List<SimpleInventoryItem>>> Inventory(int? userId, int? InventoryType)
        {
            if (userId == null)
            {
                return BadRequest();
                // return Content(NotFound().StatusCode.ToString());
            }

            Type convertedInventoryType;
            if (InventoryType == null) convertedInventoryType = typeof(Fridge);
            else
            {
                convertedInventoryType = FromEnumToType(InventoryType);
            }

            var inventory = uow.Users.GetInventoryWithUser((int)userId, typeof(Fridge));
            if (inventory == null)
            {
                return Content(NotFound().StatusCode.ToString());
            }

            //Remove unnecesary data:
            List<SimpleInventoryItem> listII = new List<SimpleInventoryItem>();
            foreach (var ii in inventory.ItemCollection)
            {
                listII.Add(new SimpleInventoryItem(ii));
                //json += JsonSerializer.Serialize( simpleInventoryItem);
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(InventoryType), InventoryType, null);
            }
        }

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
                    var simpleII = new SimpleInventoryItem(item);
                    inventoryItems.Add(simpleII);
                }
            }

            return inventoryItems;
        }



        // GET: PpUser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PpUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PpUserId,Email,Name,PasswordHash,CreationDate")]
            PpUser ppUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ppUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(ppUser);
        }

        // GET: PpUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ppUser = await _context.PpUser.FindAsync(id);
            if (ppUser == null)
            {
                return NotFound();
            }

            return View(ppUser);
        }

        // POST: PpUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PpUserId,Email,Name,PasswordHash,CreationDate")]
            PpUser ppUser)
        {
            if (id != ppUser.PpUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ppUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PpUserExists(ppUser.PpUserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(ppUser);
        }

        // GET: PpUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ppUser = await _context.PpUser
                .FirstOrDefaultAsync(m => m.PpUserId == id);
            if (ppUser == null)
            {
                return NotFound();
            }

            return View(ppUser);
        }

        // POST: PpUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ppUser = await _context.PpUser.FindAsync(id);
            _context.PpUser.Remove(ppUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PpUserExists(int id)
        {
            return _context.PpUser.Any(e => e.PpUserId == id);
        }
    }
}

