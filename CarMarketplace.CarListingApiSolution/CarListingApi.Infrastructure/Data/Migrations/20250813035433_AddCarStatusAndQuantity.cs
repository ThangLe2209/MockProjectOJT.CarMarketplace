using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarListingApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCarStatusAndQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Car",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Car",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Quantity", "Status" },
                values: new object[] { 3, "Available" });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Quantity", "Status" },
                values: new object[] { 2, "Available" });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Quantity", "Status" },
                values: new object[] { 1, "Available" });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Quantity", "Status" },
                values: new object[] { 1, "Available" });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Quantity", "Status" },
                values: new object[] { 1, "Available" });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Quantity", "Status" },
                values: new object[] { 1, "Available" });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Quantity", "Status" },
                values: new object[] { 1, "Available" });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Quantity", "Status" },
                values: new object[] { 1, "Available" });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Quantity", "Status" },
                values: new object[] { 1, "Available" });

            migrationBuilder.UpdateData(
                table: "Car",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Quantity", "Status" },
                values: new object[] { 1, "Available" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Car");
        }
    }
}
