﻿using System.Linq;
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

        public MyDbContext PlutoContext
        {
            get { return Context as MyDbContext; }
        }


        public async Task<int> GetPpUserIdByJWT(string authorization)
        {
            string jwt = authorization.Split(" ")[1];
            var dbUser =await Context.User.Include(u => u.PpUser).SingleOrDefaultAsync(u => u.AccessJWTToken == jwt);
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
    }
}
