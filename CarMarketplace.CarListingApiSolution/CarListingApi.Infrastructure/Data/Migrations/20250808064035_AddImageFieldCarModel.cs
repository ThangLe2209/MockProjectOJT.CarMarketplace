using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarListingApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFieldCarModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Car",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: "https://hips.hearstapps.com/hmg-prod/amv-prod-cad-assets/images/17q3/685270/2018-toyota-camry-se-25l-test-review-car-and-driver-photo-691169-s-original.jpg");

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Image", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 5, 20, 0, 42, 59, 0, DateTimeKind.Local), "https://cdn.jdpower.com/ChromeImageGallery/Expanded/Transparent/640/2020HOC18_640/2020HOC180001_640_01.png", new DateTime(2024, 5, 20, 0, 42, 59, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Image", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 5, 20, 1, 42, 59, 0, DateTimeKind.Local), "https://www.kbb.com/wp-content/uploads/2014/07/2015-ford-f-150-xlt-front-static-600-001.jpg", new DateTime(2024, 5, 20, 1, 42, 59, 0, DateTimeKind.Local) });

            migrationBuilder.InsertData(
                table: "Car",
                columns: new[] { "Id", "Color", "CreatedDate", "Description", "Image", "Make", "Mileage", "Model", "Price", "SellerId", "Title", "UpdatedDate", "Year" },
                values: new object[,]
                {
                    { 4, "White", new DateTime(2024, 5, 20, 2, 42, 59, 0, DateTimeKind.Local), "Comfortable sedan with advanced safety features and smooth ride.", "https://vexstockimages.fastly.carvana.io/stockimages/2019_Chevrolet_Malibu_LT%20Sedan%204D_WHITE_stock_mobile_640x640.png", "Chevrolet", 38000, "Malibu", 17999.00m, 2, "2019 Chevrolet Malibu LT", new DateTime(2024, 5, 20, 2, 42, 59, 0, DateTimeKind.Local), 2019 },
                    { 5, "Black", new DateTime(2024, 5, 20, 3, 42, 59, 0, DateTimeKind.Local), "Reliable midsize sedan with great fuel efficiency.", "https://images.automatrix.com/1/99228/rKnx6gGpepMj.jpg", "Nissan", 60000, "Altima", 15500.00m, 1, "2017 Nissan Altima S", new DateTime(2024, 5, 20, 3, 42, 59, 0, DateTimeKind.Local), 2017 },
                    { 6, "White", new DateTime(2024, 5, 20, 4, 42, 59, 0, DateTimeKind.Local), "Electric sedan with autopilot and premium interior.", "https://static.cargurus.com/images/forsale/2025/05/28/16/05/2021_tesla_model_3-pic-7064985328337502694-1024x768.jpeg", "Tesla", 12000, "Model 3", 37999.00m, 1, "2021 Tesla Model 3 Standard Range Plus", new DateTime(2024, 5, 20, 4, 42, 59, 0, DateTimeKind.Local), 2021 },
                    { 7, "Gray", new DateTime(2024, 5, 20, 5, 42, 59, 0, DateTimeKind.Local), "Luxury sedan with sporty handling and premium features.", "https://images.hgmsites.net/lrg/2016-bmw-3-series-4-door-sedan-328i-rwd-angular-front-exterior-view_100545095_l.jpg", "BMW", 52000, "320i", 20999.00m, 1, "2016 BMW 3 Series 320i", new DateTime(2024, 5, 20, 5, 42, 59, 0, DateTimeKind.Local), 2016 },
                    { 8, "Blue", new DateTime(2024, 5, 20, 6, 42, 59, 0, DateTimeKind.Local), "Spacious sedan with smooth ride and great value.", "https://upload.wikimedia.org/wikipedia/commons/1/1f/2014_Hyundai_Sonata_%28LF_MY14%29_Active_sedan_%282018-10-29%29_01.jpg", "Hyundai", 85000, "Sonata", 10999.00m, 2, "2014 Hyundai Sonata GLS", new DateTime(2024, 5, 20, 6, 42, 59, 0, DateTimeKind.Local), 2014 },
                    { 9, "Green", new DateTime(2024, 5, 20, 7, 42, 59, 0, DateTimeKind.Local), "All-wheel drive wagon with advanced safety and comfort.", "https://i.pinimg.com/474x/91/24/26/912426871ef2767eb2536724e01672d0.jpg", "Subaru", 29000, "Outback", 25999.00m, 1, "2019 Subaru Outback Premium", new DateTime(2024, 5, 20, 7, 42, 59, 0, DateTimeKind.Local), 2019 },
                    { 10, "Silver", new DateTime(2024, 5, 20, 8, 42, 59, 0, DateTimeKind.Local), "Compact sedan with German engineering and great mileage.", "https://primeautoomaha.com/wp-content/uploads/2023/07/1-2013-volkswagen-vw-jetta-s-silver-prime-auto-omaha.jpg", "Volkswagen", 95000, "Jetta", 8999.00m, 1, "2013 Volkswagen Jetta SE", new DateTime(2024, 5, 20, 8, 42, 59, 0, DateTimeKind.Local), 2013 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Car");

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Local), new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Local), new DateTime(2024, 5, 19, 23, 42, 59, 0, DateTimeKind.Local) });
        }
    }
}
