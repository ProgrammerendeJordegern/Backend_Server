using System;
using System.ComponentModel.DataAnnotations;


namespace DataBase.Models
{
    public interface ISimpleInventoryItem
    {
        public Item Item { get; set; }
        public uint Amount { get; set; }
        public DateTime DateAdded { get; set; }
    }
    public class InventoryItem:ISimpleInventoryItem
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
