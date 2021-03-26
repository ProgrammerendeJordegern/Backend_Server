using System.Collections.Generic;
using DataBase.Models;

namespace DataBase
{
    abstract public class IItem
    {
        abstract public int ItemId { get; set; }
        //public List<string> EanCollection { get; set; }//Flere varenumre kan være en del af et Item fx. Kærgåden og Bakkedal smørbar
        abstract public string Name { get; set; }

    }
    public class Item:IItem
    {
        //public int ItemId { get; set; }
        public override int ItemId { get; set; }

        //public List<string> EanCollection { get; set; }//Flere varenumre kan være en del af et Item fx. Kærgåden og Bakkedal smørbar
        public override string Name { get; set; }
        public ICollection<InventoryItem> InventoryCollection { get; set; } //En vare kan eksistere på flere lister (Fx smør i fryser og køleskab)

        public int amountInfridge { get; set; }
        public int amountInFreezer { get; set; }

    }
}