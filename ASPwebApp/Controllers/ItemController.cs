using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataBase;
using DataBase.Data;

namespace ASPwebApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly MyDbContext _context;
        private UnitOfWork uow;

        public ItemController(MyDbContext context)
        {
            _context = context;
            uow = new UnitOfWork(_context);
        }

   

        // GET: Item/Details/5
        public async Task<ActionResult<Item>> FromId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item =uow.Items.Get((int) id);
                //= await _context.Item
                //.FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return item;
        }
        public async Task<ActionResult<Item>> FromEan(string? ean)
        {
            if (ean == null)
            {
                return NotFound();
            }

            var item = await uow.Items.GetItemWithEan(ean);
                //_context.Item
                //.FirstOrDefaultAsync(m => m.Ean == ean);
            if (item == null)
            {
                return NotFound();
            }

            return item;
        }
        public async Task<ActionResult<Item>> FromName(string? name)
        {
            if (name == null||name.Length<3)
            {
                return NotFound();
            }
            var item= uow.Items.SingleOrDefault(i => 
                          i.Name.ToLower()==(name.ToLower())) 
                      ?? uow.Items.SingleOrDefault(i => //findes ikke et eksakt match, søges bredere:
                          i.Name.ToLower().Contains(name.ToLower()));
            //await _context.Item
                //.FirstOrDefaultAsync(m => m.Name.Contains(name));//Contains may be changed to ==
            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

 


        // GET: Item/Edit/5
        [HttpPut]
        public async Task<ActionResult<Item>> Edit(int? id,[FromBody] Item newItem)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!_context.Item.Any(i=>i.ItemId==id))
            {
                return NotFound();
            }
            uow.Items.Add(newItem);
            uow.Complete();
            return newItem;
        }

        // POST: Item/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("ItemId,Ean,Name,AverageLifespanDays,Size,DesiredMinimumAmount")] Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Accepted();
            }

            return Accepted();
        }

        // GET: Item/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }
    }
}
