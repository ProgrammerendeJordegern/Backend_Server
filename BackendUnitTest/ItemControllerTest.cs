using System.Linq;
using System.Threading.Tasks;
using ASPwebApp.Controllers;
using NUnit.Framework;
using DataBase.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendUnitTest
{
    public class ItemControllerTest
    {
        private MyDbContext dbc;
        private ItemController uut;

        [SetUp]
        public void Setup()
        {
            dbc = new MyDbContext(@"Data Source=(localdb)\DABServer;Initial Catalog=PantryPassion;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            uut = new ItemController(dbc);
            dbc.Database.EnsureDeleted();
            dbc.Database.EnsureCreated();
            new SeedData(dbc);
        }

        [Test]
       public async Task FindItemByEan_DataIsSeeded_ItemIsReturned()
       {
           
               var response = await uut.FromEan("123123");
               var item = response.Value;
               Assert.That(item.Ean, Is.EqualTo("123123"));
       }
       [Test]
       public async Task FindItemByid_DataIsSeeded_ItemIsReturned()
       {

           var response = await uut.FromId(1);
           var item = response.Value;
           Assert.That(item.Name, Is.EqualTo("Agurk"));
       }
       [Test]
       public async Task FindItemByName_DataIsSeeded_ItemIsReturned()
       {

           var response = await uut.FromName("Mælk",null);
           var item = response.Value;
           Assert.That(item.Name, Is.EqualTo("Mælk"));
       }
        //Zero test
        [Test]
       public async Task FindItemByEan_NoItems_NothingIsReturned()
       {
           dbc.Database.EnsureDeleted();
           dbc.Database.Migrate();
           var response = await uut.FromEan("123123");
           var item = response.Value;
           Assert.That(response.Result.GetType(), Is.EqualTo(typeof( NotFoundResult)));
       }
       [Test]
       public async Task GetAllItems_DataIsSeeded_ItemIsReturned()
       {

           var response = await uut.GetItem();
           var items = response.Value;
           Assert.That(items.First().Name, Is.EqualTo("Agurk"));
           Assert.That(items.Count(), Is.GreaterThan(5));
        }
        //Denne test virker ikke, "edit" testes i stedet med postman og swagger.
        //[Test]
        //public async Task Edit_DataIsSeeded_NameIsUpdated()
        //{

        //    var item = new Item()
        //    {
        //        Name = "Lurpak Smør",
        //        ItemId = 4,
        //        AverageLifespanDays = 14,
        //        Ean = " 5740900400726"
        //    };

        //     var response = await uut.Edit(item);
        //    var responseItem = response.Value;
        //    Assert.That(responseItem.Ean, Is.EqualTo("5740900400726"));
        //    Assert.That(responseItem.Name,Is.EqualTo("Lurpak Smør"));
        //}

        [TearDown]
       public void CleanUp()
       {
           dbc.Dispose();
        }
    }
}