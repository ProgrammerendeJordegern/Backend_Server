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
    public class ManageUsersController : Controller
    {
        private readonly MyDbContext _context;

        public ManageUsersController(MyDbContext context)
        {
            _context = context;
        }

        // GET: ManageUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: ManageUsers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDb = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userDb == null)
            {
                return NotFound();
            }

            return View(userDb);
        }

        // GET: ManageUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FullName,Email,PwHash")] UserDb userDb)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userDb);
        }

        // GET: ManageUsers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDb = await _context.User.FindAsync(id);
            if (userDb == null)
            {
                return NotFound();
            }
            return View(userDb);
        }

        // POST: ManageUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UserId,FullName,Email,PwHash")] UserDb userDb)
        {
            if (id != userDb.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDbExists(userDb.UserId))
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
            return View(userDb);
        }

        // GET: ManageUsers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDb = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userDb == null)
            {
                return NotFound();
            }

            return View(userDb);
        }

        // POST: ManageUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var userDb = await _context.User.FindAsync(id);
            _context.User.Remove(userDb);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDbExists(long id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
