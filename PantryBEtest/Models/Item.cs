using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataBase.Models;

namespace DataBase
{
    public interface ISimpleItem
    {
        public int ItemId { get; set; }

        public string Ean { get; set; }
        [Required]
        public string Name { get; set; }
       // public ICollection<InventoryItem> InventoryCollection { get; set; } //En vare kan eksistere på flere lister (Fx smør i fryser og køleskab)

        public uint AverageLifespanDays { get; set; }
        public uint Size { get; set; }//Mass, Volume or amount
        public string SizeUnit { get; set; }
        public uint DesiredMinimumAmount { get; set; } //Nødvendig for at genere indkøbslite
    }

    public class SimpleItem : ISimpleItem
    {
        public SimpleItem() { }

        public SimpleItem(Item i)
        {
            ItemId = i.ItemId;
            Ean = i.Ean;
            Name = i.Name;
            AverageLifespanDays = i.AverageLifespanDays;
            DesiredMinimumAmount = i.DesiredMinimumAmount;
            Size = i.Size;
            SizeUnit = i.SizeUnit;

        }
        public int ItemId { get; set; }
        public string Ean { get; set; }
        public string Name { get; set; }
        public uint AverageLifespanDays { get; set; }
        public uint Size { get; set; }
        public string SizeUnit { get; set; }
        public uint DesiredMinimumAmount { get; set; }
    }

    public class Item:ISimpleItem
    {
        public  int ItemId { get; set; }

        //public List<string> EanCollection { get; set; }//Flere varenumre kan være en del af et Item fx. Kærgåden og Bakkedal smørbar
        public string Ean { get; set; }
        [Required]
        public  string Name { get; set; }
        public ICollection<InventoryItem> InventoryCollection { get; set; } //En vare kan eksistere på flere lister (Fx smør i fryser og køleskab)

        public uint AverageLifespanDays { get; set; }
        public uint Size { get; set; }//Mass, Volume or amount
        public string SizeUnit { get; set; }
        public uint DesiredMinimumAmount { get; set; } //Nødvendig for at genere indkøbslite

    }

    public class GlobalItem
    {
        public int ItemId { get; set; }
        public string Ean { get; set; }
        public string Name { get; set; }

    }
}