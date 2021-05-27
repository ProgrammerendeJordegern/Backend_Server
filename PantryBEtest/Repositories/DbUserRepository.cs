using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataBase.Data;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories
{
    public interface IUserDbRepository : IRepository<UserDb>
    {
        public Task<int> GetPpUserIdByJWT(string authorization);
        public Task<int[]> GetInventoryIdsByJwt(string authorization);
    }


    public class UserDbRepository : Repository<UserDb>, IUserDbRepository
    {
        public UserDbRepository(MyDbContext context) : base(context)
        {
            
        }


        public async Task<int> GetPpUserIdByJWT(string authorization)
        {
            string jwt = authorization.Split(" ")[1];
            var hash = HashJwt(jwt);
            var dbUser =await Context.User.Include(u => u.PpUser).SingleOrDefaultAsync(u => u.AccessJWTToken == hash);
            if (dbUser == null) return 0;
            return dbUser.PpUserId;
        }

        public async Task<int[]> GetInventoryIdsByJwt(string authorization)
        {
            var ppUserId = await GetPpUserIdByJWT(authorization);
            var ppUser = await Context.PpUser
                .Include(p=>p.Inventories)
                .Where(p=>p.PpUserId==ppUserId)
                .SingleOrDefaultAsync();
            int[] ids = new int[ppUser.Inventories.Count];
            uint count = 0;
            foreach (var inventory in ppUser.Inventories)
            {
                ids[count] = inventory.InventoryId;
                count++;
            }

            return ids;
        }
        public static string HashJwt(string jwt)
        {
            string tempJwt = jwt + DateTime.Now.Date;
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] bytearray = encoder.GetBytes(tempJwt);
            var hash = new SHA384Managed().ComputeHash(bytearray);
            return System.Text.Encoding.Default.GetString(hash);
        }
    }
}
