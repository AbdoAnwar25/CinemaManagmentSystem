using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CenimaManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedCinemaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cinemas",
                columns: new[] { "Id", "Description", "Logo", "Name", "Status" },
                values: new object[,]
                {
                    { 1, "Premium cinema experience with large screens and comfortable seating", "cenima1.jpg", "VOX Cinemas - City Centre", true },
                    { 2, "Immersive cinematic experience with crystal clear sound and giant screens", "cenima2.jpg", "IMAX Theatre", true },
                    { 3, "Modern cinema halls featuring the latest blockbusters", "cenima3.jpg", "Galaxy Cinema", true },
                    { 4, "Luxury cinema with reclining seats and in-seat dining service", "cenima4.jpg", "VIP Gold Class", true },
                    { 5, "One of the largest cinema complexes with multiple screens", "cenima5.jpg", "City Stars Cinema", true },
                    { 6, "State-of-the-art facility for ultimate movie entertainment", "cenima6.jpg", "Megaplex Cinema", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cinemas",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
