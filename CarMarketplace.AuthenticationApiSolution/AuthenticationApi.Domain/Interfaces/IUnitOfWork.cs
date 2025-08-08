using AuthenticationApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<UserRole> UserRoles { get; }
        IRepository<OutboxEvent> OutboxEvents { get; }
        Task<int> CompleteAsync();
    }
}
