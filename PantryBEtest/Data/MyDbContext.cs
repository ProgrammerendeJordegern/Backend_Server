using System;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataBase.Data
{
    public class MyDbContext : DbContext
    {
        private readonly string _connectionString;

        //Default constructor used by Database.Program
        public MyDbContext()
        {
            // _connectionString = @"Data Source=(localdb)\DABServer;Initial Catalog=PantryPassion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            _connectionString = @"Server=tcp:i4dab.database.windows.net,1433;Initial Catalog=pantrypassion-auecei4prj4gr3;Persist Security Info=False;User ID=pantrypassionauecei4prj4gr3;Password=pantrypassion-auecei4prj4gr3;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        }
        //Constructor with string parameter used by unit-test
        public MyDbContext(string connectionString)
        {
            // _connectionString = @"Data Source=(localdb)\DABServer;Initial Catalog=PantryPassion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            _connectionString = connectionString;

        }

        //Constructor with options used in ASPwebApp
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted })
                .EnableSensitiveDataLogging();
            if (_connectionString != null && _connectionString.Length > 10) optionsBuilder.UseSqlServer(_connectionString);
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Many to many Inventory - Item
            modelBuilder.Entity<InventoryItem>()
                .HasKey(ii => new {ii.InventoryId, ii.ItemId,ii.DateAdded});
            //modelBuilder.Entity<InventoryItem>()
            //    .HasOne(ii => ii.Item)
            //    .WithMany(i => i.InventoryCollection)
            //    .HasForeignKey(ii => ii.ItemId);
            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Inventory)
                .WithMany(i => i.ItemCollection)
                .HasForeignKey(ii => ii.InventoryId);

            //modelBuilder.Entity<Item>().HasMany(i => i.InventoryCollection)
            //    .WithMany(i => i.ItemCollection)
            //    .UsingEntity<InventoryItem>(
            //        i => i.HasOne(i => i.Inventory)
            //            .WithMany().HasForeignKey(i => i.InventoryId), i => i.HasOne(i => i.Item)
            //            .WithMany().HasForeignKey(i => i.InventoryId));
            
        }

        //private void CreateSeedData(ModelBuilder mb)
        //{
        //    var i1 = new Item() { Name = "Smør", InventoryCollection = new List<InventoryItem>() };
        //    var i2 = new Item() { Name = "Mælk", InventoryCollection = new List<InventoryItem>() };
        //    var i3 = new Item() { Name = "Æg", InventoryCollection = new List<InventoryItem>() };
        //    var i4 = new Item() { Name = "Toiletpapir", InventoryCollection = new List<InventoryItem>() };

        //    var inventory1 = new Inventory() { Type = "Fridge", ItemCollection = new List<InventoryItem>() };
        //    var inventory2 = new Inventory() { Type = "Freezer", ItemCollection = new List<InventoryItem>() };
        //    var inventory3 = new Inventory() { Type = "Pantry", ItemCollection = new List<InventoryItem>() };

        //    mb.Entity<InventoryItem>().HasData(ii1);
        //    mb.Entity<InventoryItem>().HasData(ii2);
        //    mb.Entity<InventoryItem>().HasData(ii3);
        //    mb.Entity<InventoryItem>().HasData(ii4);

        //    var ii1 = new InventoryItem() { Amount = 2, Inventory = inventory1, Item = i1.ItemId };
        //    var ii2 = new InventoryItem() { Amount = 3, Inventory = inventory1, Item = i2 };
        //    var ii3 = new InventoryItem() { Amount = 1, Inventory = inventory1, Item = i3 };
        //    var ii4 = new InventoryItem() { Amount = 8, Inventory = inventory3, Item = i4 };



        //    mb.Entity<PpUser>().HasData(new PpUser()
        //    {
        //        CreationDate = DateTime.Now,
        //        Email = "john@john.dk",
        //        Inventories = new List<Inventory>(),
        //        Name = "John køkkensen",
        //        PasswordHash = "2233",
        //        PpUserId = 22
        //    });
        //    mb.Entity<Inventory>().HasData(new Inventory() { InventoryId = 11, });
        //}
        public DbSet<UserDb> User { get; set; }
        public DbSet<PpUser> PpUser { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Fridge> Fridge { get; set; }
        public DbSet<Freezer> Freezer { get; set; }
        public DbSet<Pantry> Pantry { get; set; }
        public DbSet<ShoppingList> ShoppingList { get; set; }
        public DbSet<InventoryItem> InventoryItem { get; set; }

    }
}
