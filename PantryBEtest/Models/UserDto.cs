using System.ComponentModel.DataAnnotations;

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

        [MaxLength(254)]
        public string AccessJWTToken { get; set; }

    }
}
