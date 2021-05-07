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
            List<Item> items = SeedItems(_context);
            SeedItemsIntoUserInventories(_context, items, users);
        }

        private void SeedItemsIntoUserInventories(MyDbContext context, List<Item> items, List<UserDb> users)
        {
            #region Kurt
            UserDb u1 = users[0];

            Inventory fridge = u1.PpUser.Inventories.ToList()[1];

            //context.InventoryItem.Add();



            #endregion





            UserDb u2 = users[1];

            



        }

        


        private static  List<UserDb> SeedUser(MyDbContext context)
        {
            UserDb u1 = new UserDb()
            {
                FullName = "Kurt Kurtsen",
                Email = "Kurt@kurt.com",
                PpUser = new PpUser(),
                CreationDate = DateTime.Today,
                //Password = KurtLovesPP1!
                PwHash = HashPassword("KurtLovesPP1!", BcryptWorkFactor)
            };

            UserDb u2 = new UserDb()
            {
                FullName = "Bodil Bodilsen",
                Email = "Bodil@bodil.com",
                PpUser = new PpUser(),
                CreationDate = DateTime.Today,
                //Password = BodilLovesPP1!
                PwHash = HashPassword("BodilLovesPP1!", BcryptWorkFactor)
            };

            context.Add(u1);
            context.Add(u2);
            context.SaveChangesAsync();
            Console.WriteLine("Added 2 users: Kurt and Bodil");

            List<UserDb> users = new List<UserDb>();
            users.Add(u1);
            users.Add(u2);
            return users;
        }

        private static List<Item> SeedItems(MyDbContext context)
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
                Name = "Toiletpapir",
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
