using CenimaManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CenimaManagmentSystem.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieSubImage> MovieSubImages { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Data Source=DESKTOP-8FGDB4A;initial catalog=CinemaManagmentSystem;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(

                new Category { Id = 1, Name = "Electronics", Description = "All electronic devices", Status = true },
                new Category { Id = 2, Name = "Clothes", Description = "Fashion and apparel", Status = false },
                new Category { Id = 3, Name = "Books", Description = "Educational and fiction books", Status = true },
                new Category { Id = 4, Name = "Laptops", Description = "Gaming and work laptops", Status = true }

                );

            modelBuilder.Entity<Actor>().HasData(
            new Actor { Id = 1, Name = "Leonardo DiCaprio", Description = "Famous American actor known for Titanic and Inception", Logo = "Actor1.jpg" },
            new Actor { Id = 2, Name = "Tom Cruise", Description = "American actor famous for Mission Impossible series", Logo = "Actor2.jpg" },
            new Actor { Id = 3, Name = "Robert Downey Jr.", Description = "Known for playing Iron Man in Marvel Cinematic Universe", Logo = "Actor3.jpg" },
            new Actor { Id = 4, Name = "Scarlett Johansson", Description = "Famous actress known for Black Widow", Logo = "Actor4.jpg" },
            new Actor { Id = 5, Name = "Cillian Murphy", Description = "Irish actor known for Oppenheimer and Peaky Blinders", Logo = "Actor5.jpg" },
            new Actor { Id = 6, Name = "Brad Pitt", Description = "Famous actor known for Fight Club and Se7en", Logo = "Actor6.jpg" },
            new Actor { Id = 7, Name = "Will Smith", Description = "American actor known for Men in Black and Pursuit of Happyness", Logo = "Actor7.jpg" }
        );

            modelBuilder.Entity<Cinema>().HasData(
            new Cinema { Id = 1, Name = "VOX Cinemas - City Centre", Description = "Premium cinema experience with large screens and comfortable seating", Status = true, Logo = "cenima1.jpg" },
            new Cinema { Id = 2, Name = "IMAX Theatre", Description = "Immersive cinematic experience with crystal clear sound and giant screens", Status = true, Logo = "cenima2.jpg" },
            new Cinema { Id = 3, Name = "Galaxy Cinema", Description = "Modern cinema halls featuring the latest blockbusters", Status = true, Logo = "cenima3.jpg" },
            new Cinema { Id = 4, Name = "VIP Gold Class", Description = "Luxury cinema with reclining seats and in-seat dining service", Status = true, Logo = "cenima4.jpg" },
            new Cinema { Id = 5, Name = "City Stars Cinema", Description = "One of the largest cinema complexes with multiple screens", Status = true, Logo = "cenima5.jpg" },
            new Cinema { Id = 6, Name = "Megaplex Cinema", Description = "State-of-the-art facility for ultimate movie entertainment", Status = false, Logo = "cenima6.jpg" }
            
            );


            // Seed Movies Data
            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id = 1,
                    Name = "Inception",
                    Description = "A thief who steals corporate secrets through the use of dream-sharing technology.",
                    Status = true,
                    Price = 150.00m,
                    DateTime = new DateTime(2026, 11, 1, 20, 0, 0),
                    MainImg = "Movie1.jpg",
                    CategoryId = 1,
                    CinemaId = 1
                },
                new Movie
                {
                    Id = 2,
                    Name = "Interstellar",
                    Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                    Status = true,
                    Price = 180.00m,
                    DateTime = new DateTime(2026, 11, 5, 21, 30, 0),
                    MainImg = "Movie2.jpg",
                    CategoryId = 1,
                    CinemaId = 2
                },
                new Movie
                {
                    Id = 3,
                    Name = "The Dark Knight",
                    Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham.",
                    Status = true,
                    Price = 160.00m,
                    DateTime = new DateTime(2026, 11, 10, 19, 0, 0),
                    MainImg = "Movie3.jpg",
                    CategoryId = 2,
                    CinemaId = 3
                }
            );

        }
    }
}
