using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Data;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
       
    }


    public class InventoryRepository : Repository<Inventory>, IInventoryRepository
    {
        public InventoryRepository(MyDbContext context) : base(context)
        {
        }

        public MyDbContext PlutoContext
        {
            get { return Context as MyDbContext; }
        }

        
    }
}
