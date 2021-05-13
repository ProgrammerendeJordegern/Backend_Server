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
using SQLitePCL;

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
        public async Task CreateWNewItem__IIisCreated()
        {
            try
            {
                var ii = new InventoryItem()
                {
                    Amount = 3,
                    Item = new Item()
                    {
                        Name = "juice",
                        AverageLifespanDays = 3
                    }
                };
                await uut.CreateWNewItem(null, 1, ii, "Bearer " + jwt);
                var item=dbc.Item.Single(i => i.Name == "juice");
                Assert.That(item.AverageLifespanDays, Is.EqualTo(3));

            }
            finally
            {
                await dbc.Database.EnsureDeletedAsync();
                await dbc.DisposeAsync();
            }
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
        [Test]
        public async Task Get_DataIsSeed_ResultIsOk()
        {

            try
            {
                var result = await uut.Get(1,"Bearer "+jwt);
                //uut.CreateWNewItem(null, 1, ii, "Bearer " + jwt);
                
                Assert.That(result.Value.Count, Is.EqualTo(2));
                Assert.That(result.Value[0].Item.Name,Is.EqualTo("Agurk"));
                Assert.That(result.Value[0].Amount, Is.EqualTo(2));
            }
            finally
            {
                dbc.Database.EnsureDeleted();
                dbc.Dispose();
            }
        }
        [TestCase((uint.MinValue))]
        [TestCase((uint)0)]
        [TestCase((uint)1)]
        public async Task Edit_DataIsSeed_AmountIsUpdated(uint amount)
        {
            try
            {
                var ii = new InventoryItem()
                {
                    ItemId = 1,
                    InventoryId = 2,
                    Amount = amount
                };
                var result = await uut.Edit(ii);
                var get = await uut.Get(1, "Bearer " + jwt);
               
                Assert.That(get.Value[0].Amount, Is.EqualTo(amount));
            }
            finally
            {
                dbc.Database.EnsureDeleted();
                dbc.Dispose();
            }
        }

    }
}