using System;
using System.Collections.Generic;

namespace DataBase
{
    public class PpUser
    {
        public int PpUserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
        public DateTime CreationDate { get; set; }
    }
}