using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class UserDb
    {
        public UserDb()
        {
            PpUser = new PpUser(2);
            CreationDate=DateTime.Now;
        }
        public UserDb(string fn, string email)
        {
            FullName = fn;
            Email = email;
            CreationDate = DateTime.Today;
            PpUser = new PpUser(2);
        }


        [Key] 
        public long UserId { get; set; }
        [MaxLength(96)] 
        public string FullName { get; set; }

        [MaxLength(254)] 
        public string Email { get; set; }

        [MaxLength(64)] 
        public string PwHash { get; set; }
        public DateTime CreationDate { get; set; }
        
        public string AccessJWTToken { get; set; }

        public PpUser PpUser { get; set; }
        public int PpUserId { get; set; }


    }
}
