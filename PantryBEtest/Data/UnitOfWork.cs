using System;
using DataBase.Repositories;

namespace DataBase.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IItemRepository Items { get; }
        IUserDbRepository UserDb { get; }
        IInventoryItemRepository InventoryItems { get; }
        IInventoryRepository Inventories { get; }
        int Complete();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _context;

        public UnitOfWork(MyDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            Items = new ItemRepository(_context);
            UserDb = new UserDbRepository(_context);
            Inventories = new InventoryRepository(_context);
            InventoryItems = new InventoryItemRepository(_context);
        }


        public IUserRepository Users { get; private set; }
        public IItemRepository Items { get; }
        public IUserDbRepository UserDb { get; }
        public IInventoryItemRepository InventoryItems { get; }
        public IInventoryRepository Inventories { get; }


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
