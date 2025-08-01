using CarListingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarListingApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // Your DbSets here
        DbSet<CarListing> Car { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Mock data for Cars
            var tempCurrentDate = new DateTime(2024, 05, 19, 22, 42, 59, DateTimeKind.Local);

            var cars = new List<CarListing>
            {
                new CarListing
                {
                    Id = 1,
                    Title = "2018 Toyota Camry SE",
                    Description = "Well-maintained sedan with excellent fuel economy and low mileage.",
                    Price = 18500.00m,
                    Make = "Toyota",
                    Model = "Camry",
                    Year = 2018,
                    Mileage = 45000,
                    Color = "Silver",
                    CreatedDate = tempCurrentDate.AddHours(1),
                    UpdatedDate = tempCurrentDate.AddHours(1),
                    SellerId = 1
                },
                new CarListing
                {
                    Id = 2,
                    Title = "2020 Honda Civic Sport",
                    Description = "Sporty and reliable compact car with modern features and great handling.",
                    Price = 20500.00m,
                    Make = "Honda",
                    Model = "Civic",
                    Year = 2020,
                    Mileage = 30000,
                    Color = "Blue",
                    CreatedDate = tempCurrentDate.AddHours(1),
                    UpdatedDate = tempCurrentDate.AddHours(1),
                    SellerId = 2
                },
                new CarListing
                {
                    Id = 3,
                    Title = "2015 Ford F-150 XLT",
                    Description = "Powerful pickup truck with towing package and spacious cabin.",
                    Price = 22999.99m,
                    Make = "Ford",
                    Model = "F-150",
                    Year = 2015,
                    Mileage = 75000,
                    Color = "Red",
                    CreatedDate = tempCurrentDate.AddHours(1),
                    UpdatedDate = tempCurrentDate.AddHours(1),
                    SellerId = 1
                }
            };

            modelBuilder.Entity<CarListing>().HasData(cars);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    var createdDate = ((BaseEntity)entityEntry.Entity).CreatedDate;
                    if (createdDate == DateTime.MinValue) // e.CreatedDate == default(DateTime) 
                    {
                        ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
