using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string? UserName { get; set; }

        //[MaxLength(200)]
        //public string? Password { get; set; }

        [Required]
        public bool Active { get; set; }

        [MaxLength(200)]
        public string? Email { get; set; }

        //public int UserRoleId { get; set; }
    }
}
