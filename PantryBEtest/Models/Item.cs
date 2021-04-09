using System.Collections.Generic;
using DataBase.Models;

namespace DataBase
{
  
    public class Item
    {
        //public int ItemId { get; set; }
        public  int ItemId { get; set; }

        //public List<string> EanCollection { get; set; }//Flere varenumre kan være en del af et Item fx. Kærgåden og Bakkedal smørbar
        public string Ean { get; set; }
        public  string Name { get; set; }
        public ICollection<InventoryItem> InventoryCollection { get; set; } //En vare kan eksistere på flere lister (Fx smør i fryser og køleskab)

        public uint AverageLifespanDays { get; set; }
        public uint Size { get; set; }//Mass, Volume or amount
        public uint DesiredMinimumAmount { get; set; } //Nødvendig for at genere indkøbslite

    }

    public class GlobalItem
    {
        public int ItemId { get; set; }
        public string Ean { get; set; }
        public string Name { get; set; }

    }
}