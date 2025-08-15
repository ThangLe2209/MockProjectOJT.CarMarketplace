using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Domain.Interfaces;
using AuthenticationApi.Infrastructure.Data;


namespace AuthenticationApi.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
    }
}
