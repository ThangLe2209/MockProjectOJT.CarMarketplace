using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public class UserInputDto
    {
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Input Password"), MinLength(4, ErrorMessage = "MinLength Password is 4")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Input Email"), MinLength(4, ErrorMessage = "MinLength Email is 4")]
        public string? Email { get; set; }
    }
}
