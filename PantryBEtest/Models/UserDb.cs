using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class UserDb
    {
        [Key] 
        public long UserId { get; set; }
        [MaxLength(96)] 
        public string FullName { get; set; }

        [MaxLength(254)] 
        public string Email { get; set; }

        [MaxLength(64)] 
        public string PwHash { get; set; }




    }
}
