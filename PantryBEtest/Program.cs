using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Data;
using DataBase.Models;

namespace DataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (var context = new MyDbContext())
            {
                //AddDummyData();

                using (var UOW = new UnitOfWork(new MyDbContext()))
                {
                    var inventories = UOW.Users.GetInventoriesWithUser(1);
                    var fridge = inventories.SingleOrDefault(i => i.Type.Equals("Fridge"));

                    
                    foreach (var item in fridge.ItemCollection)
                    {
                        Console.WriteLine(item.Item.Name);
                        Console.WriteLine(item.Amount);
                    }
                }

                //XmlSerializer x = new XmlSerializer(typeof(List<IItem>));
                //TextWriter writer = new StreamWriter(@"ItemList.xml");
                // x.Serialize(writer, nylist);


            }
        }

        public static void AddDummyData()
            {
                using (var context = new MyDbContext())
                {
                    var i1 = new Item() {Name = "Smør", InventoryCollection = new List<InventoryItem>()};
                    var i2 = new Item() {Name = "Mælk", InventoryCollection = new List<InventoryItem>()};
                    var i3 = new Item() {Name = "Æg", InventoryCollection = new List<InventoryItem>()};
                    var i4 = new Item() {Name = "Toiletpapir", InventoryCollection = new List<InventoryItem>()};

                    var inventory1 = new Inventory() {Type = "Fridge", ItemCollection = new List<InventoryItem>()};
                    var inventory2 = new Inventory() {Type = "Freezer", ItemCollection = new List<InventoryItem>()};
                    var inventory3 = new Inventory() {Type = "Pantry", ItemCollection = new List<InventoryItem>()};

                    var ii1 = new InventoryItem() {Amount = 2, Inventory = inventory1, Item = i1};
                    
                    var ii2 = new InventoryItem() { Amount = 3, Inventory = inventory1, Item = i2 };
                    
                    var ii3 = new InventoryItem() { Amount = 1, Inventory = inventory1, Item = i3 };
                    
                    var ii4 = new InventoryItem() { Amount = 8, Inventory = inventory3, Item = i4 }; 
                   
                    context.InventoryItem.Add(ii4);
                    context.InventoryItem.Add(ii1);
                    context.InventoryItem.Add(ii2);
                    context.InventoryItem.Add(ii3);

                

                PpUser u1 = new PpUser()
                    {
                        Email = "mail@mail.dk", 
                        Inventories = new List<Inventory>(), 
                        Name = "Børge",
                        PasswordHash = "123"
                    };

                    u1.Inventories.Add(inventory1);
                    u1.Inventories.Add(inventory2);
                    u1.Inventories.Add(inventory3);

                    context.PpUser.Add(u1);

                    context.SaveChanges();
                }
            }
        
        

        
    }
}
