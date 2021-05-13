using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ASPwebApp.Controllers;
using NUnit.Framework;
using DataBase;
using DataBase.Data;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendUnitTest
{
    public class InventoryItemControllerTest
    {
        private MyDbContext dbc;
        private InventoryItem2Controller uut;
        private string jwt="test";
        [SetUp]

        public void Setup()
        {
            dbc = new MyDbContext(new DbContextOptions<MyDbContext>());
            uut = new InventoryItem2Controller(dbc);
            dbc.Database.EnsureCreated();
            new SeedData(dbc);
            var user=dbc.User.First();
            user.AccessJWTToken = jwt;
            dbc.SaveChanges();
        }

        [Test]
        public async Task CreateWExistingItem_II_already_Created_tody_originalIsIncemted()
        {
            
           
            try
            {
                //uut.CreateWNewItem(null, 1, ii, "Bearer " + jwt);
                var ii = new SimpleInventoryItem()
                {
                    Amount = 3,
                    ItemId = 2
                };
                var result = await uut.CreateWExistingItem(null, 2, ii, "Bearer " + jwt); 
                result = await uut.CreateWExistingItem(null, 2, ii, "Bearer " + jwt);
                var result1 = await uut.Get(2, "Bearer " + jwt);
                Assert.That(result1.Value.Count, Is.EqualTo(2));//Two II already exists with seed data
                Assert.That(result1.Value[0].Amount,Is.EqualTo(8));

            }
            finally
            {
                dbc.Database.EnsureDeleted();
                dbc.Dispose();
            }
        }

    }
}