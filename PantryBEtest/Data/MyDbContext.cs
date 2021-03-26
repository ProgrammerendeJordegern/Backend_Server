using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Data
{
    public class MyDbContext : DbContext
    {
        //public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=(localdb)\DABServer;Initial Catalog=PantryPassion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Many to many Inventory - Item
            modelBuilder.Entity<InventoryItem>()
                .HasKey(ii => new {ii.InventoryId, ii.ItemId});
            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Item)
                .WithMany(i => i.InventoryCollection)
                .HasForeignKey(ii => ii.ItemId);
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

        public DbSet<PpUser> PpUser { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<InventoryItem> InventoryItem { get; set; }

    }
}
