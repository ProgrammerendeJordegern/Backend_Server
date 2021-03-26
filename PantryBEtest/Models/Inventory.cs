using System.Collections.Generic;
using DataBase.Models;

namespace DataBase
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public string Type { get; set; }
        public ICollection<InventoryItem> ItemCollection { get; set; }
    }
}