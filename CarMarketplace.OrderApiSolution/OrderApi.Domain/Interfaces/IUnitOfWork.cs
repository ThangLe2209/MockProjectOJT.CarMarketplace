using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Order> Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IRepository<OutboxEvent> OutboxEvents { get; }
        IRepository<ProcessedEvent> ProcessedEvents { get; }
        Task<int> CompleteAsync();
    }
}
