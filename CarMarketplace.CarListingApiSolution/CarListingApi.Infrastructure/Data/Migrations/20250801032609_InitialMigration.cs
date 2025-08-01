using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarListingApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Car",
                columns: new[] { "Id", "Color", "CreatedDate", "Description", "Make", "Mileage", "Model", "Price", "SellerId", "Title", "UpdatedDate", "Year" },
                values: new object[,]
                {
                    { 1, "Silver", new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Local), "Well-maintained sedan with excellent fuel economy and low mileage.", "Toyota", 45000, "Camry", 18500.00m, 1, "2018 Toyota Camry SE", new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Local), 2018 },
                    { 2, "Blue", new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Local), "Sporty and reliable compact car with modern features and great handling.", "Honda", 30000, "Civic", 20500.00m, 2, "2020 Honda Civic Sport", new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Local), 2020 },
                    { 3, "Red", new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Local), "Powerful pickup truck with towing package and spacious cabin.", "Ford", 75000, "F-150", 22999.99m, 1, "2015 Ford F-150 XLT", new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Local), 2015 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Car");
        }
    }
}
