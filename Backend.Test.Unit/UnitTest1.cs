using DataBase.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Test.Unit
{
    public class UnitTest1
    {
        private readonly UnitOfWork uut;
        private readonly MyDbContext dbcontext;

        public UnitTest1()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<MyDbContext>().UseSqlite(connection).Options;

            dbcontext = new MyDbContext(options);

            uut = new WeatherObservationsController(dbcontext);
        }
        [Fact]
        public void Test1()
        {

        }
    }
}
