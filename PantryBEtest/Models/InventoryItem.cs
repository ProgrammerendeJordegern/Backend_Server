using System;
using Microsoft.VisualBasic;

namespace DataBase.Models
{
    public class InventoryItem
    {
        public int ItemId { get; set; }
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        public Item Item { get; set; }
        public uint Amount { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
