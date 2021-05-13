using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
        private Item2Controller uut;
        [SetUp] 
       
        public void Setup()
        {
             dbc = new MyDbContext(new DbContextOptions<MyDbContext>());
            uut = new Item2Controller(dbc);
            dbc.Database.EnsureCreated();
        }

        
    }
}