using System;
using System.Collections.Generic;
using System.Linq;
using DataBase.Data;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Pantry Passion Backend Server.");

            using (var context = new MyDbContext())
            {
                
                using (var UOW = new UnitOfWork(new MyDbContext()))
                {
                    var user = UOW.Users.GetUserWithEmail("mail@mail.dk");
                    if (user == null)
                    {
                        Console.WriteLine("Adding Dummydata...");
                        AddDummyData();
                        Console.WriteLine("\n\nDummydata Added");
                    }
                }
                    
                

                
                using (var UOW = new UnitOfWork(new MyDbContext()))
                {
                    Console.WriteLine("Getting inventories:");
                    var inventories = UOW.Users.GetInventoriesWithUser(23);
                    if (inventories != null) { 
                        var fridge = inventories.SingleOrDefault(i => i.GetType() == typeof(Fridge));
                        Console.WriteLine(inventories.ElementAt(0).GetType());
                        //Print content in fridge
                        Console.WriteLine("Indhold i køleskab: ");
                        foreach (var item in fridge.ItemCollection)
                        {
                            Console.WriteLine(item.Item.Name+ ": "+ item.Amount);
                        }
                    }
                    

                //Search for user and change name
                Console.WriteLine("Indtast bruger ID:");
                    var input = Console.ReadLine();
                    PpUser user = UOW.Users.Get(int.Parse(input));
                    if (user != null) { 
                    Console.WriteLine("Name: "+user.Name+" Type new Name: ");
                    input = Console.ReadLine();
                    user.Name = input;
                    UOW.Complete();
                    }
                    else Console.WriteLine("Could not find user.");

                    //Get passwordHash
                    PpUser u2 = UOW.Users.GetUserWithEmail("mail@mail.dk");
                    Console.WriteLine(u2.PasswordHash);
                }

               


            }
        }

        public static void AddDummyData()
            {
                using (var context = new MyDbContext())
                {
                    var i1 = new Item() {Name = "Smør", InventoryCollection = new List<InventoryItem>(),Ean = "123123"};
                    var i2 = new Item() {Name = "Mælk", InventoryCollection = new List<InventoryItem>()};
                    var i3 = new Item() {Name = "Æg", InventoryCollection = new List<InventoryItem>()};
                    var i4 = new Item() {Name = "Toiletpapir", InventoryCollection = new List<InventoryItem>()};

                    var inventory1 = new Fridge() { ItemCollection = new List<InventoryItem>()};
                    var inventory2 = new Freezer() { ItemCollection = new List<InventoryItem>()};
                    var inventory3 = new Pantry() { ItemCollection = new List<InventoryItem>()};

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


                    PpUser u2 = new PpUser()
                    {
                        Email = "kurt@kurt.dk",
                        Inventories = new List<Inventory>(),
                        Name = "Kurt Jespersen",
                        PasswordHash = "22333"
                    };
                    Inventory inventory4 = new ShoppingList() {ItemCollection = new List<InventoryItem>()};
                    Inventory inventory5 = new Fridge() { ItemCollection = new List<InventoryItem>() };
                    var ii5 = new InventoryItem() { Amount = 20, Inventory = inventory4, Item = i1 };
                    var ii6 = new InventoryItem() { Amount = 10, Inventory = inventory5, Item = i2 };
                    inventory5.ItemCollection.Add(ii5);
                    inventory5.ItemCollection.Add(ii6);

                    context.PpUser.Add(u2);

                    context.SaveChanges();
                }

            }
        
        

        
    }
}
