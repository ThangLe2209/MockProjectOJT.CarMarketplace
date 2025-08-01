using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Domain.Entities
{
    public class UserClaim
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        [Required]
        public required string Type { get; set; }

        [MaxLength(250)]
        [Required]
        public required string Value { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
