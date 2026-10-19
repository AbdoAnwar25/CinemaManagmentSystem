namespace CenimaManagmentSystem.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
        public string MainImg { get; set; }

        public List<String> SubImages { get; set; } = new List<string>();
        public List<Actor> Actors { get; set; } = new List<Actor>();



        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
    }
}
