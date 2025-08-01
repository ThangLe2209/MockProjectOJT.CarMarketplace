using AuthenticationApi.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public class UserDto : BaseEntity
    {
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
        public UserRoleDto? UserRole { get; set; }
        public ICollection<UserClaimDto> Claims { get; set; } = new List<UserClaimDto>();
        //public ICollection<TodoItemHistoryDto> TodoItemHistories { get; set; } = new List<TodoItemHistoryDto>();
    }
}
