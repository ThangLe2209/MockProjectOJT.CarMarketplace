using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarListingApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgreMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Make = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Mileage = table.Column<int>(type: "integer", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    SellerId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessedEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedEvent", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Car",
                columns: new[] { "Id", "Color", "CreatedDate", "Description", "Image", "IsDeleted", "Make", "Mileage", "Model", "Price", "Quantity", "SellerId", "Status", "Title", "UpdatedDate", "Year" },
                values: new object[,]
                {
                    { 1, "Silver", new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Utc), "Well-maintained sedan with excellent fuel economy and low mileage.", "https://hips.hearstapps.com/hmg-prod/amv-prod-cad-assets/images/17q3/685270/2018-toyota-camry-se-25l-test-review-car-and-driver-photo-691169-s-original.jpg", false, "Toyota", 45000, "Camry", 18500.00m, 3, 1, "Available", "2018 Toyota Camry SE", new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Utc), 2018 },
                    { 2, "Blue", new DateTime(2024, 5, 20, 0, 42, 59, 0, DateTimeKind.Utc), "Sporty and reliable compact car with modern features and great handling.", "https://cdn.jdpower.com/ChromeImageGallery/Expanded/Transparent/640/2020HOC18_640/2020HOC180001_640_01.png", false, "Honda", 30000, "Civic", 20500.00m, 2, 2, "Available", "2020 Honda Civic Sport", new DateTime(2024, 5, 20, 0, 42, 59, 0, DateTimeKind.Utc), 2020 },
                    { 3, "Red", new DateTime(2024, 5, 20, 1, 42, 59, 0, DateTimeKind.Utc), "Powerful pickup truck with towing package and spacious cabin.", "https://www.kbb.com/wp-content/uploads/2014/07/2015-ford-f-150-xlt-front-static-600-001.jpg", false, "Ford", 75000, "F-150", 22999.99m, 1, 1, "Available", "2015 Ford F-150 XLT", new DateTime(2024, 5, 20, 1, 42, 59, 0, DateTimeKind.Utc), 2015 },
                    { 4, "White", new DateTime(2024, 5, 20, 2, 42, 59, 0, DateTimeKind.Utc), "Comfortable sedan with advanced safety features and smooth ride.", "https://vexstockimages.fastly.carvana.io/stockimages/2019_Chevrolet_Malibu_LT%20Sedan%204D_WHITE_stock_mobile_640x640.png", false, "Chevrolet", 38000, "Malibu", 17999.00m, 1, 2, "Available", "2019 Chevrolet Malibu LT", new DateTime(2024, 5, 20, 2, 42, 59, 0, DateTimeKind.Utc), 2019 },
                    { 5, "Black", new DateTime(2024, 5, 20, 3, 42, 59, 0, DateTimeKind.Utc), "Reliable midsize sedan with great fuel efficiency.", "https://images.automatrix.com/1/99228/rKnx6gGpepMj.jpg", false, "Nissan", 60000, "Altima", 15500.00m, 1, 1, "Available", "2017 Nissan Altima S", new DateTime(2024, 5, 20, 3, 42, 59, 0, DateTimeKind.Utc), 2017 },
                    { 6, "White", new DateTime(2024, 5, 20, 4, 42, 59, 0, DateTimeKind.Utc), "Electric sedan with autopilot and premium interior.", "https://static.cargurus.com/images/forsale/2025/05/28/16/05/2021_tesla_model_3-pic-7064985328337502694-1024x768.jpeg", false, "Tesla", 12000, "Model 3", 37999.00m, 1, 1, "Available", "2021 Tesla Model 3 Standard Range Plus", new DateTime(2024, 5, 20, 4, 42, 59, 0, DateTimeKind.Utc), 2021 },
                    { 7, "Gray", new DateTime(2024, 5, 20, 5, 42, 59, 0, DateTimeKind.Utc), "Luxury sedan with sporty handling and premium features.", "https://images.hgmsites.net/lrg/2016-bmw-3-series-4-door-sedan-328i-rwd-angular-front-exterior-view_100545095_l.jpg", false, "BMW", 52000, "320i", 20999.00m, 1, 1, "Available", "2016 BMW 3 Series 320i", new DateTime(2024, 5, 20, 5, 42, 59, 0, DateTimeKind.Utc), 2016 },
                    { 8, "Blue", new DateTime(2024, 5, 20, 6, 42, 59, 0, DateTimeKind.Utc), "Spacious sedan with smooth ride and great value.", "https://upload.wikimedia.org/wikipedia/commons/1/1f/2014_Hyundai_Sonata_%28LF_MY14%29_Active_sedan_%282018-10-29%29_01.jpg", false, "Hyundai", 85000, "Sonata", 10999.00m, 1, 2, "Available", "2014 Hyundai Sonata GLS", new DateTime(2024, 5, 20, 6, 42, 59, 0, DateTimeKind.Utc), 2014 },
                    { 9, "Green", new DateTime(2024, 5, 20, 7, 42, 59, 0, DateTimeKind.Utc), "All-wheel drive wagon with advanced safety and comfort.", "https://i.pinimg.com/474x/91/24/26/912426871ef2767eb2536724e01672d0.jpg", false, "Subaru", 29000, "Outback", 25999.00m, 1, 1, "Available", "2019 Subaru Outback Premium", new DateTime(2024, 5, 20, 7, 42, 59, 0, DateTimeKind.Utc), 2019 },
                    { 10, "Silver", new DateTime(2024, 5, 20, 8, 42, 59, 0, DateTimeKind.Utc), "Compact sedan with German engineering and great mileage.", "https://primeautoomaha.com/wp-content/uploads/2023/07/1-2013-volkswagen-vw-jetta-s-silver-prime-auto-omaha.jpg", false, "Volkswagen", 95000, "Jetta", 8999.00m, 1, 1, "Available", "2013 Volkswagen Jetta SE", new DateTime(2024, 5, 20, 8, 42, 59, 0, DateTimeKind.Utc), 2013 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "ProcessedEvent");
        }
    }
}
