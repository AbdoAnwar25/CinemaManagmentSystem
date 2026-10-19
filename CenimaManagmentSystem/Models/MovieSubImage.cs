namespace CenimaManagmentSystem.Models
{
    public class MovieSubImage
    {
        public int Id { get; set; }
        public string Image { get; set; }

        // Foreign Key للفيلم
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}