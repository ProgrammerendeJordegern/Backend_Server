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

namespace ASPwebApp.Controllers
{
    public class PpUserController : Controller
    {
        private readonly MyDbContext _context;

        public PpUserController(MyDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Details(string? email)
        {
            if (email == null)
            {
                return NotFound();
            }

            UnitOfWork uow = new UnitOfWork(_context);
            var ppUser = uow.Users.GetUserWithEmail(email);
            if (ppUser == null)
            {
                return NotFound();
            }

            return View(ppUser);
        }

        public async Task<IActionResult> Inventory(int? userId, Type? InventoryType,[FromHeader]string username,[FromHeader] string password)
        {
            if (username == "flemming") return Content("HAHAHA");
            if (userId == null)
            {
                return Content(NotFound().StatusCode.ToString());
            }

            
            if (InventoryType == null) InventoryType = typeof(Fridge);
            

            UnitOfWork uow = new UnitOfWork(_context);
            var inventory = uow.Users.GetInventoryWithUser((int)userId, typeof(Fridge));
            if (inventory == null)
            {
                return Content(NotFound().StatusCode.ToString());
            }

            //return HTML page
            //return View( inventory.ItemCollection);

            //Remove unnecesary data:
            string json="";
            foreach (var ii in inventory.ItemCollection)
            {
                ISimpleInventoryItem simpleInventoryItem = new SimpleInventoryItem(ii);
                json += JsonSerializer.Serialize( simpleInventoryItem);
            }
            return Content(json);
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
        public async Task<IActionResult> Create([Bind("PpUserId,Email,Name,PasswordHash,CreationDate")] PpUser ppUser)
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
        public async Task<IActionResult> Edit(int id, [Bind("PpUserId,Email,Name,PasswordHash,CreationDate")] PpUser ppUser)
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
