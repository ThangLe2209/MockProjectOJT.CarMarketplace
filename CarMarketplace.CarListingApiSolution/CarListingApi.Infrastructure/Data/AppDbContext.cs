using CarListingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarListingApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // Your DbSets here
        DbSet<CarListing> Car { get; set; }
        DbSet<ProcessedEvent> ProcessedEvent { get; set; }


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
                    Image = "https://hips.hearstapps.com/hmg-prod/amv-prod-cad-assets/images/17q3/685270/2018-toyota-camry-se-25l-test-review-car-and-driver-photo-691169-s-original.jpg",
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
                    Image = "https://cdn.jdpower.com/ChromeImageGallery/Expanded/Transparent/640/2020HOC18_640/2020HOC180001_640_01.png",
                    CreatedDate = tempCurrentDate.AddHours(2),
                    UpdatedDate = tempCurrentDate.AddHours(2),
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
                    Image = "https://www.kbb.com/wp-content/uploads/2014/07/2015-ford-f-150-xlt-front-static-600-001.jpg",
                    CreatedDate = tempCurrentDate.AddHours(3),
                    UpdatedDate = tempCurrentDate.AddHours(3),
                    SellerId = 1
                },
                new CarListing
                {
                    Id = 4,
                    Title = "2019 Chevrolet Malibu LT",
                    Description = "Comfortable sedan with advanced safety features and smooth ride.",
                    Price = 17999.00m,
                    Make = "Chevrolet",
                    Model = "Malibu",
                    Year = 2019,
                    Mileage = 38000,
                    Color = "White",
                    Image = "https://vexstockimages.fastly.carvana.io/stockimages/2019_Chevrolet_Malibu_LT%20Sedan%204D_WHITE_stock_mobile_640x640.png",
                    CreatedDate = tempCurrentDate.AddHours(4),
                    UpdatedDate = tempCurrentDate.AddHours(4),
                    SellerId = 2
                },
                new CarListing
                {
                    Id = 5,
                    Title = "2017 Nissan Altima S",
                    Description = "Reliable midsize sedan with great fuel efficiency.",
                    Price = 15500.00m,
                    Make = "Nissan",
                    Model = "Altima",
                    Year = 2017,
                    Mileage = 60000,
                    Color = "Black",
                    Image = "https://images.automatrix.com/1/99228/rKnx6gGpepMj.jpg",
                    CreatedDate = tempCurrentDate.AddHours(5),
                    UpdatedDate = tempCurrentDate.AddHours(5),
                    SellerId = 1
                },
                new CarListing
                {
                    Id = 6,
                    Title = "2021 Tesla Model 3 Standard Range Plus",
                    Description = "Electric sedan with autopilot and premium interior.",
                    Price = 37999.00m,
                    Make = "Tesla",
                    Model = "Model 3",
                    Year = 2021,
                    Mileage = 12000,
                    Color = "White",
                    Image = "https://static.cargurus.com/images/forsale/2025/05/28/16/05/2021_tesla_model_3-pic-7064985328337502694-1024x768.jpeg",
                    CreatedDate = tempCurrentDate.AddHours(6),
                    UpdatedDate = tempCurrentDate.AddHours(6),
                    SellerId = 1
                },
                new CarListing
                {
                    Id = 7,
                    Title = "2016 BMW 3 Series 320i",
                    Description = "Luxury sedan with sporty handling and premium features.",
                    Price = 20999.00m,
                    Make = "BMW",
                    Model = "320i",
                    Year = 2016,
                    Mileage = 52000,
                    Color = "Gray",
                    Image = "https://images.hgmsites.net/lrg/2016-bmw-3-series-4-door-sedan-328i-rwd-angular-front-exterior-view_100545095_l.jpg",
                    CreatedDate = tempCurrentDate.AddHours(7),
                    UpdatedDate = tempCurrentDate.AddHours(7),
                    SellerId = 1
                },
                new CarListing
                {
                    Id = 8,
                    Title = "2014 Hyundai Sonata GLS",
                    Description = "Spacious sedan with smooth ride and great value.",
                    Price = 10999.00m,
                    Make = "Hyundai",
                    Model = "Sonata",
                    Year = 2014,
                    Mileage = 85000,
                    Color = "Blue",
                    Image = "https://upload.wikimedia.org/wikipedia/commons/1/1f/2014_Hyundai_Sonata_%28LF_MY14%29_Active_sedan_%282018-10-29%29_01.jpg",
                    CreatedDate = tempCurrentDate.AddHours(8),
                    UpdatedDate = tempCurrentDate.AddHours(8),
                    SellerId = 2
                },
                new CarListing
                {
                    Id = 9,
                    Title = "2019 Subaru Outback Premium",
                    Description = "All-wheel drive wagon with advanced safety and comfort.",
                    Price = 25999.00m,
                    Make = "Subaru",
                    Model = "Outback",
                    Year = 2019,
                    Mileage = 29000,
                    Color = "Green",
                    Image = "https://i.pinimg.com/474x/91/24/26/912426871ef2767eb2536724e01672d0.jpg",
                    CreatedDate = tempCurrentDate.AddHours(9),
                    UpdatedDate = tempCurrentDate.AddHours(9),
                    SellerId = 1
                },
                new CarListing
                {
                    Id = 10,
                    Title = "2013 Volkswagen Jetta SE",
                    Description = "Compact sedan with German engineering and great mileage.",
                    Price = 8999.00m,
                    Make = "Volkswagen",
                    Model = "Jetta",
                    Year = 2013,
                    Mileage = 95000,
                    Color = "Silver",
                    Image = "https://primeautoomaha.com/wp-content/uploads/2023/07/1-2013-volkswagen-vw-jetta-s-silver-prime-auto-omaha.jpg",
                    CreatedDate = tempCurrentDate.AddHours(10),
                    UpdatedDate = tempCurrentDate.AddHours(10),
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
