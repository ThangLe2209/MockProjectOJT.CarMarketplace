using CarListingApi.Domain.Entities;
using CarListingApi.Domain.Interfaces;
using CarListingApi.Infrastructure.Data;

namespace CarListingApi.Infrastructure.Repositories
{
    public class CarRepository : Repository<CarListing>, ICarRepository
    {
        public CarRepository(AppDbContext context) : base(context) { }


    }
}
