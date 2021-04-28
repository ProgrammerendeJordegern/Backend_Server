using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;
using DataBase;
using DataBase.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendUnitTest
{
    public class DataBaseTests
    {
        private UnitOfWork uow;
        private MyDbContext dbc;
        [SetUp] 
       
        public void Setup()
        {
            dbc = new MyDbContext(new DbContextOptions<MyDbContext>());
            uow = new UnitOfWork(dbc);
            dbc.Database.EnsureCreated();
        }

        [Test]
        public void DbContext_DataBaseIsAlreadyCreated()
        {
            Assert.That(dbc.Database.EnsureCreated(),Is.False);
        }
        [Test]
        public void PPUser_InsertSeedData_CheckIfExits()
        {
            try
            {
                DataBase.Program.AddDummyData(new MyDbContext(new DbContextOptions<MyDbContext>()));
                Assert.That(dbc.PpUser.Count(), Is.EqualTo(2));
            }
            finally
            {
                dbc.Database.EnsureDeleted();
                dbc.Dispose();
            }
        }
        [Test]
        public void GetUserWithEmail_WithSeedData_CheckFridgeSize()
        {
            try
            {
                DataBase.Program.AddDummyData(new MyDbContext(new DbContextOptions<MyDbContext>()));
                var user = uow.Users.GetUserWithEmail("mail@mail.dk");
                
                Assert.That(user.Email, Is.EqualTo("mail@mail.dk"));
            }
            finally
            {
                dbc.Database.EnsureDeleted();
                dbc.Dispose();
            }
        }
        [Test]
        public void GetInventoriesWithUser_WithSeedData_CheckFridgeSize()
        {
            try
            {
                DataBase.Program.AddDummyData(new MyDbContext(new DbContextOptions<MyDbContext>()));
                var user = uow.Users.GetUserWithEmail("mail@mail.dk");
                var inventories = uow.Users.GetInventoriesWithUser(user.PpUserId);
                var fridge = inventories.Single(i => i is Fridge);
                Assert.That(fridge.ItemCollection.Count, Is.EqualTo(3));
            }
            finally
            {
                dbc.Database.EnsureDeleted();
                dbc.Dispose();
            }
        }

        [Test]
        public void UOW_WithSeedData_CheckItemsCorrect()
        {
            try
            {
                DataBase.Program.AddDummyData(new MyDbContext(new DbContextOptions<MyDbContext>()));
                var user = uow.Users.GetUserWithEmail("mail@mail.dk");
                var fridge = uow.Users.GetInventoryWithUser(user.PpUserId,typeof(Fridge));
                Assert.That(fridge.ItemCollection.Count, Is.EqualTo(3));
            }
            finally
            {
                dbc.Database.EnsureDeleted();
                dbc.Dispose();
            }
        }
    }
}