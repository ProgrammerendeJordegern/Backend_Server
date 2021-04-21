using System;
using System.ComponentModel.DataAnnotations;


namespace DataBase.Models
{
    public interface ISimpleInventoryItem
    {
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public ISimpleItem Item { get; set; }
        public uint Amount { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class SimpleInventoryItem : ISimpleInventoryItem
    {
        public SimpleInventoryItem() { }
        public SimpleInventoryItem(InventoryItem ii)
        {
            InventoryId = ii.InventoryId;
            ItemId = ii.ItemId;
            Item = ii.Item;
            Amount = ii.Amount;
            DateAdded = ii.DateAdded;
        }

        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public ISimpleItem Item { get; set; }
        public uint Amount { get; set; }
        public DateTime DateAdded { get; set; }

        public static implicit operator SimpleInventoryItem(InventoryItem v)
        {
            throw new NotImplementedException();
        }
    }

    public class InventoryItem
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        public Item Item { get; set; }
        public uint Amount { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
