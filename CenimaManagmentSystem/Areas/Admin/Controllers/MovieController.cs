using CenimaManagmentSystem.Data;
using CenimaManagmentSystem.Models;
using CenimaManagmentSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CenimaManagmentSystem.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _Context = new ApplicationDbContext();

        //اندكس التطبيق
        //public IActionResult Index()
        //{

        //    var movies = _Context.Movies.AsQueryable();

        //    return View(movies.AsEnumerable());
        //}


        // اندكس التاسك الي فيها فيلتر ب الديت و الباجينيشن
        public IActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5; // عدد العناصر في كل صفحة (ممكن تغيرها براحتك)
            var movies = _Context.Movies.AsQueryable();

            // 1. لو في بحث بالتاريخ


            // حساب إجمالي عدد العناصر عشان نعرف هنعمل كم صفحة (لو احتجناها)
            int totalItems = movies.Count();

            // 2. تطبيق الـ Pagination (تقسيم الصفحات)
            var pagedMovies = movies
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            // بنبعت رقم الصفحة الحالية وإجمالي الصفحات للـ View عشان أزرار التنقل
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var joinmovies = _Context.Movies
            .Include(m => m.Category)
            .Include(m => m.Cinema)
            .ToList();

            return View(pagedMovies);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var categories = _Context.Categories.ToList();
            var cinemas = _Context.Cinemas.ToList();

            return View( new MovieVM() 
            {
               Categories = categories ,
               Cinemas = cinemas
                
            });
        }

        [HttpPost]   ////////////////////////////////////هنزود الجزئيه بتاعه ال subimages => List<IformFile>
        public IActionResult Create(Movie movie, IFormFile ImageFile , List<IFormFile> subImages) // الشرح تحت 
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + ImageFile.FileName;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var Stream = System.IO.File.Create(filePath))
                {
                    ImageFile.CopyTo(Stream);
                }

                movie.MainImg = fileName;

            }
            var SavedMovie =  _Context.Movies.Add(movie);
            _Context.SaveChanges();

            // نكمل جزئيه ال subimages 

            if (subImages != null && subImages.Count > 0)
            {
                foreach(var image in subImages)
                {

                    //adding in wwwroot
                    if (image != null && image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + "-" + image.FileName;

                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images","MovieSubImages", fileName);

                        using (var Stream = System.IO.File.Create(filePath))
                        {
                            image.CopyTo(Stream);
                        }

                        //adding in DataBase
                        _Context.MovieSubImages.Add(new MovieSubImage()
                        {
                            Image = fileName,
                            MovieId = SavedMovie.Entity.Id
                        });
                    }
                }
                _Context.SaveChanges();

            }
                

                return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Update(int Id)
        {
            var movie = _Context.Movies.FirstOrDefault(c => c.Id == Id);
            if (movie == null)
            {
                return NotFound();
            }
            MovieVM movieVM = new MovieVM()
            {
                Movie = movie,
                Categories = _Context.Categories.ToList(),
                Cinemas = _Context.Cinemas.ToList()
            };
            return View(movieVM);
        }

        [HttpPost]
        public IActionResult Update(MovieVM movieVM, IFormFile ImageFile)
        {
            //عشان اجيب الداتا من الداتا بيز
            var MovieInDb = _Context.Movies.AsNoTracking().FirstOrDefault(b => b.Id == movieVM.Movie.Id);

            // اللفه بتاعه ابلود الفايلات او الصور
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + "-" + ImageFile.FileName;
                //بكريت اسم جديد للفايل زائد اسم الفايل
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                //عشان لو نقلت المشروع علي جهاز تاني
                using (var Steam = System.IO.File.Create(filePath))//
                {
                    ImageFile.CopyTo(Steam);
                }

                movieVM.Movie.MainImg = fileName;// بعدل الاسم في الداتا بيز

                // دي عشان لما اخد فايل او صوره جديده يمسح القديم عشان المساحه
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", MovieInDb.MainImg);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            else
            {
                movieVM.Movie.MainImg = MovieInDb.MainImg;
            }


            _Context.Movies.Update(MovieInDb);
            _Context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }









        public IActionResult Delete(int Id)
        {
            var movie = _Context.Movies.FirstOrDefault(c => c.Id == Id);

            // دي عشان لما اخد فايل او صوره جديده يمسح القديم عشان المساحه
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", movie.MainImg);

            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }

            _Context.Movies.Remove(movie);
            _Context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
