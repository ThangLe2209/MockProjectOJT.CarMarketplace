using CarListingApi.Domain.Entities;
using CarListingApi.Domain.Interfaces;
using CarListingApi.Infrastructure.Data;


namespace CarListingApi.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ICarRepository? _carRepository;
        private IRepository<ProcessedEvent>? _processedEventRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ICarRepository Cars => _carRepository ??= new CarRepository(_context);
        public IRepository<ProcessedEvent> ProcessedEvents => _processedEventRepository ??= new Repository<ProcessedEvent>(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    }
}
