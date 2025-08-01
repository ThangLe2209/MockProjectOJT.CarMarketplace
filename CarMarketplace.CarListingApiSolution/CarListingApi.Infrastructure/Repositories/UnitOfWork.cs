using CarListingApi.Domain.Entities;
using CarListingApi.Domain.Interfaces;
using CarListingApi.Infrastructure.Data;


namespace CarListingApi.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<CarListing>? _carRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<CarListing> Cars => _carRepository ??= new Repository<CarListing>(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    }
}
