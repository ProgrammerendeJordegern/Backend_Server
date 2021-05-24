using System;
using System.Linq;
using System.Threading.Tasks;
using ASPwebApp.Controllers;
using DataBase;
using NUnit.Framework;
using DataBase.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendUnitTest
{
    public class IInventoryControllerTest
    {
        private MyDbContext dbc;
        private InventoryController uut;
        private string jwt = "test";
        private string authorization;

        [SetUp]
        public void Setup()
        {
            dbc = new MyDbContext(@"Data Source=(localdb)\DABServer;Initial Catalog=PantryPassion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            uut = new InventoryController(dbc);
            dbc.Database.EnsureDeleted();
            dbc.Database.Migrate();
            new SeedData(dbc);
            //login:
            var user = dbc.User.First();
            user.AccessJWTToken = jwt;
            dbc.SaveChanges();
            authorization   = "Bearer " + jwt;
        }

        [Test]
       public async Task GetAllInventories_DataIsSeeded_InventoryItemsAreReturned()
       {
           
               var response = await uut.GetAllInventories(authorization);
               var items = response.Value;
               Assert.That(items.Count, Is.GreaterThan(5));
               Assert.That(items.First().Item.Name,Is.EqualTo("Is"));
       }
       [Test]
       public async Task GetFridge_DataIsSeeded_ItemsAreInFridge()
       {

           var response = await uut.Inventory(1,authorization);
           var items = response.Value;
           Assert.That(items.Count, Is.GreaterThan(5));
           Assert.That(items.First().InventoryType, Is.EqualTo(1));
       }
       [Test]
       public async Task GetInvalidType_DataIsSeeded_ErrorReturned()
       {
          var result= await uut.Inventory(5, authorization);
          Assert.That(result.Result,Is.TypeOf(typeof(BadRequestObjectResult)));
       }

       [TestCase(0,typeof(Freezer))]
       [TestCase(1, typeof(Fridge))]
       [TestCase(2, typeof(Pantry))]
       [TestCase(3, typeof(ShoppingList))]

        public async Task FromEnumToType_MultipleInput_ReturnIsCorrect(int input,Type expectedType)
        {
            var result = InventoryController.FromEnumToType(input);
            Assert.That(result, Is.EqualTo(expectedType));
        }

        [TestCase(-1)]
        [TestCase(5)]
        [TestCase(100)]

        public async Task FromEnumToType_WrongInput_ErrorIsTrown(int input)
        {
            Assert.Throws<ArgumentOutOfRangeException>( () => InventoryController.FromEnumToType(input));

        }
        [TestCase(100)]

        public async Task DeleteAllContentInAnInventory_DataIsSeeded_ListIsEmpty(int input)
        {
            await uut.DeleteAllContentInAnInventory(0, authorization);
            var count =dbc.PpUser.First().Inventories.First().ItemCollection.Count;
            Assert.That(count,Is.Zero);
        }

        [TearDown]
       public void CleanUp()
       {
           dbc.Dispose();
        }
    }
}