using CenimaManagmentSystem.Models;

namespace CenimaManagmentSystem.ViewModels
{
    public class MovieVM
    {

        public List<Category> Categories { get; set; }
        public List<Cinema> Cinemas { get; set; }
        public Movie Movie { get; internal set; }
    }
}
