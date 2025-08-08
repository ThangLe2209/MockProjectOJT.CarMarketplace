using CarListingApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarListingApi.Application.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetUserAsync(int userId);
    }
}
