using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthenticationController(IAuthenticationService authenticationService, IMapper mapper, IConfiguration configuration)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpGet("{userId}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserByIdAsync(int userId)
        {
            var user = await _authenticationService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new BadRequestResponse(HttpContext.Request.Path, "User not existed"));
            }

            return Ok(new SuccessResponse<UserDto>(user, HttpContext.Request.Path));
        }

        [HttpGet("test")]
        public IActionResult TestCICD()
        {
            return Ok("Test CICD");
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUserAsync(UserInputDto userInput)
        {
            userInput.Password = BCrypt.Net.BCrypt.HashPassword(userInput.Password);
            var createdUserToReturn = await _authenticationService.CreateUserAsync(userInput);
            return CreatedAtRoute("GetUserById",
                new
                {
                    userId = createdUserToReturn.Id,
                }
                , new SuccessResponse<UserDto>(createdUserToReturn, HttpContext.Request.Path, "User created successfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserLoginDto userLogin)
        {
            var user = await _authenticationService.GetUserByInfoAsync(_mapper.Map<UserInputDto>(userLogin));

            if (user == null)
            {
                return NotFound(new BadRequestResponse(HttpContext.Request.Path, "Userinfo is not correct!"));
            }

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password);
            if (!verifyPassword) return NotFound(new BadRequestResponse(HttpContext.Request.Path, "Invalid Password credential!"));

            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            await _authenticationService.SaveRefreshTokenAsync(user.Id, refreshToken);

            return Ok(new SuccessResponse<object>(
                new { AccessToken = accessToken, RefreshToken = refreshToken },
                HttpContext.Request.Path, "Login successful"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            await _authenticationService.SoftDeleteUserAsync(id);
            return NoContent();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            //var refreshToken = Request.Cookies["RefreshToken"]; // Get refresh token from cookie
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return Unauthorized(new { Message = "Refresh token missing!" });
            }

            var user = await _authenticationService.GetUserByRefreshTokenAsync(request.RefreshToken);
            if (user is null)
            {
                return Unauthorized(new { Message = "Invalid or expired refresh token!" });
            }

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // Save the new refresh token
            await _authenticationService.SaveRefreshTokenAsync(user.Id, newRefreshToken);

            return Ok(new SuccessResponse<object>(
                 new { AccessToken = newAccessToken, RefreshToken = newRefreshToken },
                 HttpContext.Request.Path, "RefreshToken Update successful"));
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.UserRole!.Value),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}
