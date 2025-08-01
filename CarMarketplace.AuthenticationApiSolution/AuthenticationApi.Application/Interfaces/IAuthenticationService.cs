

using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Domain.Entities;

namespace AuthenticationApi.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> CheckUserInfoAsync(UserInputDto userInfo);
        Task<User?> GetUserByInfoAsync(UserInputDto userInfo);
        Task SaveRefreshTokenAsync(int userId, string refreshToken);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto> CreateUserAsync(UserInputDto userInput);
    }
}
