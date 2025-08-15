using OrderApi.Domain.Entities;
using OrderApi.Domain.Interfaces;
using OrderApi.Infrastructure.Data;

namespace OrderApi.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<Order>? _orderRepository;
        private IRepository<OrderItem>? _orderItemRepository;
        private IRepository<OutboxEvent>? _outboxEventRepository;
        private IRepository<ProcessedEvent>? _processedEventRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<Order> Orders => _orderRepository ??= new Repository<Order>(_context);
        public IRepository<OrderItem> OrderItems => _orderItemRepository ??= new Repository<OrderItem>(_context);
        public IRepository<OutboxEvent> OutboxEvents => _outboxEventRepository ??= new Repository<OutboxEvent>(_context);
        public IRepository<ProcessedEvent> ProcessedEvents => _processedEventRepository ??= new Repository<ProcessedEvent>(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    }
}
