using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CenimaManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedActorsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "Description", "Logo", "MovieId", "Name" },
                values: new object[,]
                {
                    { 1, "Famous American actor known for Titanic and Inception", "Actor1.jpg", null, "Leonardo DiCaprio" },
                    { 2, "American actor famous for Mission Impossible series", "Actor2.jpg", null, "Tom Cruise" },
                    { 3, "Known for playing Iron Man in Marvel Cinematic Universe", "Actor3.jpg", null, "Robert Downey Jr." },
                    { 4, "Famous actress known for Black Widow", "Actor4.jpg", null, "Scarlett Johansson" },
                    { 5, "Irish actor known for Oppenheimer and Peaky Blinders", "Actor5.jpg", null, "Cillian Murphy" },
                    { 6, "Famous actor known for Fight Club and Se7en", "Actor6.jpg", null, "Brad Pitt" },
                    { 7, "American actor known for Men in Black and Pursuit of Happyness", "Actor7.jpg", null, "Will Smith" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
