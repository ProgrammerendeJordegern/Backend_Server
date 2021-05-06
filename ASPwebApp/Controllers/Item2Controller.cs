﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataBase;
using DataBase.Data;

namespace ASPwebApp.Controllers
{
    [Route("api/Item")]
    [ApiController]
    public class Item2Controller : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly UnitOfWork uow;
        public Item2Controller(MyDbContext context)
        {
            _context = context;
            uow = new UnitOfWork(_context);
        }
        /// <summary>
        /// Autogenerated get list of items (Associated with user)
        /// </summary>
        /// <returns></returns>
        // GET: api/Item2
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItem()
        {
            return await _context.Item.ToListAsync();
        }
        /// <summary>
        /// Get 1 item by DB item Id
        /// </summary>
        /// <param name="id">item id as listed in DB</param>
        /// <returns></returns>
        // GET: Item/Details/5
        [HttpGet("byId/{id}")]
        public async Task<ActionResult<SimpleItem>> FromId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = uow.Items.Get((int)id);
            //= await _context.Item
            //.FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }
            var simpleItem = new SimpleItem(item);
            return simpleItem;
        }
        /// <summary>
        /// Get 1 item by barcode
        /// </summary>
        /// <param name="ean"></param>
        /// <returns></returns>
        [HttpGet("byEan/{ean}")]
        public async Task<ActionResult<SimpleItem>> FromEan(string? ean)
        {
            if (ean == null)
            {
                return NotFound();
            }

            var item = await uow.Items.GetItemWithEan(ean);
            if (item == null)
            {
                return NotFound();
            }
            var simpleItem = new SimpleItem(item);
            return simpleItem;
        }
        /// <summary>
        /// Get 1 item by name
        /// </summary>
        /// <param name="name">Currently using "contains(ToLower)"</param>
        /// <returns></returns>
        [HttpGet("byName/{name}")]
        public async Task<ActionResult<SimpleItem>> FromName(string? name)
        {
            if (name == null || name.Length < 3)
            {
                return NotFound();
            }
            var item = uow.Items.SingleOrDefault(i =>
                           i.Name.ToLower() == (name.ToLower()))
                      ?? uow.Items.SingleOrDefault(i => //findes ikke et eksakt match, søges bredere:
                          i.Name.ToLower().Contains(name.ToLower()));
            //await _context.Item
            //.FirstOrDefaultAsync(m => m.Name.Contains(name));//Contains may be changed to ==
            if (item == null)
            {
                return NotFound();
            }
            var simpleItem = new SimpleItem(item);
            return simpleItem;
        }



        /// <summary>
        /// Edit details about item
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        // GET: Item/Edit/5
        [HttpPut]
        public async Task<ActionResult<Item>> Edit([FromBody] Item newItem)
        {
            if (newItem == null)
            {
                return NotFound();
            }

            if (newItem.ItemId == null)
            {
                return NotFound();
            }



            _context.Entry(newItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(newItem.ItemId))
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

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }
    }
}
