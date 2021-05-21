using System;
using System.ComponentModel.DataAnnotations;

public enum InventoryTypes {Freezer,Fridge,Pantry,ShoppingList}
namespace DataBase.Models
{
    public interface ISimpleInventoryItem
    {
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public ISimpleItem Item { get; set; }
        public uint Amount { get; set; }
        public DateTime DateAdded { get; set; }
        public InventoryTypes InventoryType { get; set; }
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
            if (ii.Inventory != null) InventoryType = ConvertTypeToEnum(ii.Inventory.GetType());
        }
        private InventoryTypes ConvertTypeToEnum(Type t)
        {
            if (typeof(Freezer) == t) return InventoryTypes.Freezer;
            if (typeof(Fridge) == t) return InventoryTypes.Fridge;
            if (typeof(Pantry) == t) return InventoryTypes.Pantry;
            if (typeof(ShoppingList) == t) return InventoryTypes.ShoppingList;
            throw new Exception("Wrong inventory type chosen");
        }

        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public ISimpleItem Item { get; set; }
        public uint Amount { get; set; }
        public DateTime DateAdded { get; set; }
        public InventoryTypes InventoryType { get; set; }

        
    }

    public class InventoryItem
    {
        public InventoryItem() { }

        public InventoryItem(SimpleInventoryItem s)
        {
            this.InventoryId = s.InventoryId;
            this.ItemId=s.ItemId;
            this.Amount = s.Amount;
        }
        public int InventoryItemId { get; set; }
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
