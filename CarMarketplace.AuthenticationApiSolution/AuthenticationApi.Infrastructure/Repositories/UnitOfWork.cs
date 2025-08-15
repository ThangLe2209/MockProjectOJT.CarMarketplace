using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Domain.Interfaces;
using AuthenticationApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IUserRepository? _userRepository;
        private IRepository<UserRole>? _userRoleRepository;
        private IRepository<OutboxEvent>? _outboxEventRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public IRepository<UserRole> UserRoles => _userRoleRepository ??= new Repository<UserRole>(_context);
        public IRepository<OutboxEvent> OutboxEvents => _outboxEventRepository ??= new Repository<OutboxEvent>(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    }
}
