using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Models
{
    public class UserDto
    {
        [MaxLength(96)]
        public string FullName { get; set; }

        [MaxLength(254)]
        public string Email { get; set; }

        [MaxLength(72)]
        public string Password { get; set; }

    }
}
