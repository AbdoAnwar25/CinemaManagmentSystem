using CenimaManagmentSystem.Models;

namespace CenimaManagmentSystem.Repository
{
    public interface IMovieSubImageRepository : IRepository<MovieSubImage>
    {
        void RemoveRange(IEnumerable<MovieSubImage> movieSubImages);
    }
}
