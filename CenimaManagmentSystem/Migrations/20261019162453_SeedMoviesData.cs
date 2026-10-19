using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CenimaManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoviesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "CategoryId", "CinemaId", "DateTime", "Description", "MainImg", "Name", "Price", "Status", "SubImages" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 11, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), "A thief who steals corporate secrets through the use of dream-sharing technology.", "cenima1.jpg", "Inception", 150.00m, true, "[]" },
                    { 2, 1, 2, new DateTime(2026, 11, 5, 21, 30, 0, 0, DateTimeKind.Unspecified), "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.", "cenima2.jpg", "Interstellar", 180.00m, true, "[]" },
                    { 3, 2, 3, new DateTime(2026, 11, 10, 19, 0, 0, 0, DateTimeKind.Unspecified), "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham.", "cenima3.jpg", "The Dark Knight", 160.00m, true, "[]" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
