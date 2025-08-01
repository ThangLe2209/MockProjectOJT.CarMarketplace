using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public class UserLoginDto
    {

        [Required(ErrorMessage = "Input Email"), MinLength(4, ErrorMessage = "MinLength Email is 4")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Input Password"), MinLength(4, ErrorMessage = "MinLength Password is 4")]
        public string? Password { get; set; }


    }
}
