using System.Linq;
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
        private InventoryItemController uut;
        private string jwt="test";
        [SetUp]

        public async Task Setup()
        {
            dbc = new MyDbContext(@"Data Source=(localdb)\DABServer;Initial Catalog=PantryPassion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            uut = new InventoryItemController(dbc);
            await dbc.Database.EnsureDeletedAsync();

            dbc.Database.EnsureCreated();
            new SeedData(dbc);
            //login:
            var user=dbc.User.First();
            user.AccessJWTToken = AccountsController.HashJwt(jwt);
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
                await dbc.DisposeAsync();
            }
        }
        [Test]
        public async Task CreateWExistingItem_IIalreadyCreatedToday_OriginalIsIncemted()
        {
            
            try
            {
                //uut.CreateWNewItem(null, 1, ii, "Bearer " + jwt);
                var ii = new SimpleInventoryItem()
                {
                    Amount = 3,
                    ItemId = 2
                };
                var result = await uut.CreateWExistingItem(null, 3, ii, "Bearer " + jwt); 
                result = await uut.CreateWExistingItem(null, 3, ii, "Bearer " + jwt);
                var result1 = await uut.Get(2, "Bearer " + jwt);
                Assert.That(result1.Value.Count, Is.EqualTo(2));//Two II already exists with seed data
                Assert.That(result1.Value[1].Amount,Is.EqualTo(10));

            }
            finally
            {
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
                dbc.Dispose();
            }
        }

    }
}