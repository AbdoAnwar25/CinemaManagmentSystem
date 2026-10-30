using CenimaManagmentSystem.Data;
using CenimaManagmentSystem.Models;

namespace CenimaManagmentSystem.Repository
{
    public class MovieSubImageRepository : Repository<MovieSubImage> , IMovieSubImageRepository
    {
        public MovieSubImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void RemoveRange(IEnumerable<MovieSubImage> movieSubImages )
        {
            _Context.RemoveRange(movieSubImages);
        }
    }
}
