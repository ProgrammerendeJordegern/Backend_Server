﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase.Data;
using DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories
{
    public interface IUserDbRepository : IRepository<UserDb>
    {
        public Task<int> GetPpUserIdByJWT(string jwt);
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
            var dbUser =await Context.User.Include(u => u.PpUser).SingleAsync(u => u.AccessJWTToken == jwt);
           
            return dbUser.PpUserId;
        }
    }
}