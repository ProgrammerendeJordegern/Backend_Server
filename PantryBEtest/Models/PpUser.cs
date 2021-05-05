using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataBase
{
    public class PpUser
    {
        public int PpUserId { get; set; }
        public ICollection<Inventory> Inventories { get; set; }

        //Constructor
        public PpUser()
        {
            Inventories = new List<Inventory>();
            Inventories.Add(new Freezer());
            Inventories.Add(new Fridge());
            Inventories.Add(new Pantry());
            Inventories.Add(new ShoppingList());
        }



    }
}