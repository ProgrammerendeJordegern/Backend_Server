using System.Collections.Generic;
using DataBase.Models;

namespace DataBase
{
    public abstract class Inventory
    {
        public Inventory()
        {
            ItemCollection = new List<InventoryItem>();
        }
        public int InventoryId { get; set; }
       // public string Type { get; set; }
        public ICollection<InventoryItem> ItemCollection { get; set; }
    }
    public class Fridge : Inventory { }
    public class Freezer : Inventory { }
    public class Pantry : Inventory { }
    public class ShoppingList : Inventory { }
}