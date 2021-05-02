using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataBase
{
    public class PpUser
    {
        public int PpUserId { get; set; }
        [Required]
        //Email, Name, Password needs to be removed here 
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
        public DateTime CreationDate { get; set; }
    }
}