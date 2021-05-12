using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BCrypt.Net.BCrypt;
using DataBase.Models;

namespace DataBase.Data
{
    public class SeedData
    {
        private const int BcryptWorkFactor = 10;
        public SeedData(MyDbContext _context)
        {
            List<UserDb> users = SeedUser(_context);
            List<Item> fridgeItems = SeedFridgeItems(_context);
            List<Item> freezerItems = SeedFreezerItems(_context);
            List<Item> pantryItems = SeedPantryItems(_context);
            List<Item> shoppinglistItems = SeedShoppingListItems(_context);
            SeedItemsIntoUserInventories(_context, users, freezerItems, fridgeItems, pantryItems, shoppinglistItems);
        }

        private void SeedItemsIntoUserInventories(MyDbContext context, List<UserDb> users, List<Item> freezerItems, List<Item> fridgeItems, List<Item> pantryItems, List<Item> shoppinglistItems)
        {
            #region Kurt
            UserDb u1 = users[0];

            //Freezer
            Inventory freezerU1 = u1.PpUser.Inventories.ToList()[0];
            foreach (var item in freezerItems)
            {
                freezerU1.ItemCollection.Add(new InventoryItem() { Amount = 3, DateAdded = DateTime.Now, Inventory = freezerU1, Item = item });
            }

            //Fridge
            Inventory fridgeU1 = u1.PpUser.Inventories.ToList()[1];
            foreach (var item in fridgeItems)
            {
                fridgeU1.ItemCollection.Add(new InventoryItem() { Amount = 2, DateAdded = DateTime.Now, Inventory = fridgeU1, Item = item });
            }


            //Pantry
            Inventory pantryU1 = u1.PpUser.Inventories.ToList()[2];
            foreach (var item in pantryItems)
            {
                pantryU1.ItemCollection.Add(new InventoryItem() { Amount = 1, DateAdded = DateTime.Now, Inventory = pantryU1, Item = item });
            }

            //Shoppinglist
            Inventory shoppinglistU1 = u1.PpUser.Inventories.ToList()[3];
            foreach (var item in fridgeItems)
            {
                shoppinglistU1.ItemCollection.Add(new InventoryItem() { Amount = 4, DateAdded = DateTime.Now, Inventory = shoppinglistU1, Item = item });
            }

            context.SaveChanges();
            //                context.InventoryItem.Add(new InventoryItem(){Amount = 2,DateAdded = DateTime.Now});


            #endregion

            #region Bodil

            UserDb u2 = users[1];

            //Freezer
            Inventory freezerU2 = u2.PpUser.Inventories.ToList()[0];
            foreach (var item in freezerItems)
            {
                freezerU2.ItemCollection.Add(new InventoryItem() { Amount = 3, DateAdded = DateTime.Now, Inventory = freezerU2, Item = item });
            }

            //Fridge
            Inventory fridgeU2 = u2.PpUser.Inventories.ToList()[1];
            foreach (var item in fridgeItems)
            {
                fridgeU2.ItemCollection.Add(new InventoryItem() { Amount = 2, DateAdded = DateTime.Now, Inventory = fridgeU2, Item = item });
            }


            //Pantry
            Inventory pantryU2 = u2.PpUser.Inventories.ToList()[2];
            foreach (var item in pantryItems)
            {
                pantryU2.ItemCollection.Add(new InventoryItem() { Amount = 1, DateAdded = DateTime.Now, Inventory = pantryU2, Item = item });
            }

            //Shoppinglist
            Inventory shoppinglistU2 = u2.PpUser.Inventories.ToList()[3];
            foreach (var item in fridgeItems)
            {
                shoppinglistU2.ItemCollection.Add(new InventoryItem() { Amount = 4, DateAdded = DateTime.Now, Inventory = shoppinglistU2, Item = item });
            }

            context.SaveChanges();

            #endregion

        }




        private static List<UserDb> SeedUser(MyDbContext context)
        {
            UserDb u1 = new UserDb()
            {
                FullName = "Kurt Kurtsen",
                Email = "Kurt@kurt.com",
                PpUser = new PpUser(2),
                CreationDate = DateTime.Today,
                //Password = KurtLovesPP1!
                PwHash = HashPassword("KurtLovesPP1!", BcryptWorkFactor)
            };

            UserDb u2 = new UserDb()
            {
                FullName = "Bodil Bodilsen",
                Email = "Bodil@bodil.com",
                PpUser = new PpUser(2),
                CreationDate = DateTime.Today,
                //Password = BodilLovesPP1!
                PwHash = HashPassword("BodilLovesPP1!", BcryptWorkFactor)
            };

            context.Add(u1);
            context.Add(u2);
            context.SaveChanges();
            Console.WriteLine("Added 2 users: Kurt and Bodil");

            List<UserDb> users = new List<UserDb>();
            users.Add(u1);
            users.Add(u2);
            return users;
        }

        private static List<Item> SeedFridgeItems(MyDbContext context)
        {
            var i1 = new Item()
            {
                Name = "Smør",
                InventoryCollection = new List<InventoryItem>(),
                Ean = "123123"
            };
            var i2 = new Item()
            {
                Name = "Mælk",
                InventoryCollection = new List<InventoryItem>()
            };
            var i3 = new Item()
            {
                Name = "Æg",
                InventoryCollection = new List<InventoryItem>()
            };
            var i4 = new Item()
            {
                Name = "Agurk",
                InventoryCollection = new List<InventoryItem>()
            };

            context.Add(i1);
            context.Add(i2);
            context.Add(i3);
            context.Add(i4);
            context.SaveChanges();

            Console.WriteLine("Items added");

            List<Item> items = new List<Item>();
            items.Add(i1);
            items.Add(i2);
            items.Add(i3);
            items.Add(i4);

            return items;

        }

        private static List<Item> SeedFreezerItems(MyDbContext context)
        {
            var i1 = new Item()
            {
                Name = "Is",
                InventoryCollection = new List<InventoryItem>(),
                Ean = "321321"
            };
            var i2 = new Item()
            {
                Name = "Rundstykker",
                InventoryCollection = new List<InventoryItem>()
            };
            var i3 = new Item()
            {
                Name = "Kylling",
                InventoryCollection = new List<InventoryItem>()
            };
            var i4 = new Item()
            {
                Name = "Hakket Spinat",
                InventoryCollection = new List<InventoryItem>()
            };

            context.Add(i1);
            context.Add(i2);
            context.Add(i3);
            context.Add(i4);
            context.SaveChanges();

            Console.WriteLine("Items added");

            List<Item> items = new List<Item>();
            items.Add(i1);
            items.Add(i2);
            items.Add(i3);
            items.Add(i4);

            return items;

        }

        private static List<Item> SeedPantryItems(MyDbContext context)
        {
            var i1 = new Item()
            {
                Name = "Toiletpapir",
                InventoryCollection = new List<InventoryItem>(),
                Ean = "321123"
            };
            var i2 = new Item()
            {
                Name = "Tandpasta",
                InventoryCollection = new List<InventoryItem>()
            };
            var i3 = new Item()
            {
                Name = "Køkkenrulle",
                InventoryCollection = new List<InventoryItem>()
            };
            var i4 = new Item()
            {
                Name = "Husholdningsfilm",
                InventoryCollection = new List<InventoryItem>()
            };

            context.Add(i1);
            context.Add(i2);
            context.Add(i3);
            context.Add(i4);
            context.SaveChanges();

            Console.WriteLine("Items added");

            List<Item> items = new List<Item>();
            items.Add(i1);
            items.Add(i2);
            items.Add(i3);
            items.Add(i4);

            return items;

        }

        private static List<Item> SeedShoppingListItems(MyDbContext context)
        {
            var i1 = new Item()
            {
                Name = "Mel",
                InventoryCollection = new List<InventoryItem>(),
                Ean = "123123"
            };
            var i2 = new Item()
            {
                Name = "Mælk",
                InventoryCollection = new List<InventoryItem>()
            };
            var i3 = new Item()
            {
                Name = "Tomater",
                InventoryCollection = new List<InventoryItem>()
            };
            var i4 = new Item()
            {
                Name = "Vanilje is",
                InventoryCollection = new List<InventoryItem>()
            };

            context.Add(i1);
            context.Add(i2);
            context.Add(i3);
            context.Add(i4);
            context.SaveChanges();

            Console.WriteLine("Items added");

            List<Item> items = new List<Item>();
            items.Add(i1);
            items.Add(i2);
            items.Add(i3);
            items.Add(i4);

            return items;

        }
    }

}
