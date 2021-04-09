using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;
using DataBase;
using DataBase.Data;

namespace BackendUnitTest
{
    public class Tests
    {
        private UnitOfWork uut;
        [SetUp] 
       
        public void Setup()
        {
            MyDbContext dbc = new MyDbContext();
            uut = new UnitOfWork(dbc);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}