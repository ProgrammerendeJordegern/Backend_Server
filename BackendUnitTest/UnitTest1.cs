using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;
using DataBase;
using DataBase.Data;
using Microsoft.EntityFrameworkCore;

namespace BackendUnitTest
{
    public class Tests
    {
        private UnitOfWork uut;
        private MyDbContext dbc;
        [SetUp] 
       
        public void Setup()
        {
             dbc = new MyDbContext(new DbContextOptions<MyDbContext>());
            uut = new UnitOfWork(dbc);
            dbc.Database.EnsureCreated();
        }

        [Test]
        public void DataBaseIsCreated()
        {
            Assert.That(dbc.Database.EnsureCreated(),Is.False);
        }
        [Test]
        public void InsertSeedDataCheckIfExits()
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
    }
}