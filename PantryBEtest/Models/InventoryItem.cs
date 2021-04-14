using System;
using System.ComponentModel.DataAnnotations;


namespace DataBase.Models
{
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
