using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Domain.Entities
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string? UserName { get; set; }

        [MaxLength(200)]
        public string? Password { get; set; }

        [Required]
        public bool Active { get; set; }

        [MaxLength(200)]
        public string? Email { get; set; }

        public int UserRoleId { get; set; }
        public UserRole? UserRole { get; set; }
        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
