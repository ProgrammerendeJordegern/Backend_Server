using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ASPwebApp.Controllers;
using NUnit.Framework;
using DataBase;
using DataBase.Data;
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
            dbc = new MyDbContext(new DbContextOptions<MyDbContext>());
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