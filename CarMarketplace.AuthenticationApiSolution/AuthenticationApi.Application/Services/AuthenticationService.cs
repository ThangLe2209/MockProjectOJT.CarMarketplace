using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Domain.Interfaces;
using AutoMapper;
using CarMarketplace.Contracts.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publisher;

        public AuthenticationService(IUnitOfWork unitOfWork, IMapper mapper, IPublishEndpoint publisher)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        public async Task<bool> CheckUserInfoAsync(UserInputDto userInfo)
            => await _unitOfWork.Users.GetAll().AnyAsync(u => u.Email!.Equals(userInfo.Email) && u.Password!.Equals(userInfo.Password));

        public async Task<User?> GetUserByInfoAsync(UserInputDto userInfo)
            => await _unitOfWork.Users.GetAll().Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Email!.Equals(userInfo.Email));

        public async Task SaveRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _unitOfWork.Users.GetAll().FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
            {
                throw new Exception("User not found!");
            }

            // Save refresh token & expiry time
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Token valid for 7 days

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var user = await _unitOfWork.Users.GetAll().Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);

            return user; // Return user if token is valid, null otherwise
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateUserAsync(UserInputDto userInput)
        {
            var checkUsername = await _unitOfWork.Users.ExistsAsync(u => u.UserName!.ToLower().Equals(userInput.UserName!.ToLower()) || u.Email!.ToLower().Equals(userInput.Email!.ToLower()));
            if (checkUsername) throw new Exception("Username or email already exists");
            var finalUser = _mapper.Map<User>(userInput);
            await _unitOfWork.Users.AddAsync(finalUser);
            await _unitOfWork.CompleteAsync();

            // Publish event
            await _publisher.Publish(new UserRegisteredEvent
            {
                UserId = finalUser.Id,
                Email = finalUser.Email!
            });

            return _mapper.Map<UserDto>(finalUser);
        }
    }
}
