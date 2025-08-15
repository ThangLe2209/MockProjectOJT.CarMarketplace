using Refit;
using CarListingApi.Application.DTOs;


namespace CarListingApi.Application.Services
{
    public interface IAuthenticationApi
    {
        [Get("/api/authentication/{userId}")]
        Task<DTOs.ApiResponse<UserDto>> GetUserAsync(int userId);

        // Add more endpoints as needed
    }
}
