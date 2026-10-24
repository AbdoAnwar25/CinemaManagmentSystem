using CenimaManagmentSystem.Data;
using CenimaManagmentSystem.Models;
using CenimaManagmentSystem.Repository;
using CenimaManagmentSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CenimaManagmentSystem.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class MovieController : Controller
    {
        ///private readonly ApplicationDbContext _Context = new ApplicationDbContext();
        private readonly Repository<Movie> _MovieRepository = new Repository<Movie>();
        private readonly Repository<Category> _CategoryRepository = new Repository<Category>();
        private readonly Repository<Cinema> _CinemaRepository = new Repository<Cinema>();
        private readonly Repository<MovieSubImage> _MovieSubImageRepository = new Repository<MovieSubImage>();

        //اندكس التطبيق
        //public IActionResult Index()
        //{

        //    var movies = _Context.Movies.AsQueryable();

        //    return View(movies.AsEnumerable());
        //}


        // اندكس التاسك الي فيها فيلتر ب الديت و الباجينيشن
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            int pageSize = 5; // عدد العناصر في كل صفحة (ممكن تغيرها براحتك)
          //var movies = _Context.Movies.AsQueryable();
            var movies =await _MovieRepository.GetListAsync();

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

            ///var joinmovies = _Context.Movies
            ///.Include(m => m.Category)
            ///.Include(m => m.Cinema)
            ///.ToList();
            var joinmovies =await _MovieRepository.GetListAsync(includes: [m => m.Category, m => m.Cinema]);

            return View(pagedMovies);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //var categories = _Context.Categories.ToList();
            //var cinemas = _Context.Cinemas.ToList();

            var categories =await _CategoryRepository.GetListAsync();
            var cinemas =await _CinemaRepository.GetListAsync();

            return View( new MovieVM() 
            {
               Categories = categories.ToList() ,
               Cinemas = cinemas.ToList()
                
            });
        }


        [HttpPost]   ////////////////////////////////////هنزود الجزئيه بتاعه ال subimages => List<IformFile>
        public async Task<IActionResult> Create(Movie movie, IFormFile ImageFile , List<IFormFile> subImages) // الشرح تحت 
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
            ///var SavedMovie =  _Context.Movies.Add(movie);
            ///_Context.SaveChanges();
            var SavedMovie = await _MovieRepository.AddAsync(movie);
                             await _MovieRepository.CommitAsync(); 


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
                        ///_Context.MovieSubImages.Add(new MovieSubImage()
                        await _MovieSubImageRepository.AddAsync(new MovieSubImage()
                        {
                            Image = fileName,
                            MovieId = SavedMovie.Entity.Id
                        });
                    }
                }
                //_Context.SaveChanges();
                await _MovieSubImageRepository.CommitAsync();

            }
                
                return RedirectToAction(nameof(Index));
        }






        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            ///var movie = _Context.Movies.FirstOrDefault(c => c.Id == Id);
            var movie =await _MovieRepository.GetOneAsync(c => c.Id == Id);
            if (movie == null)
            {
                return RedirectToAction("NotFoundPage" , "Home");
            }

            ///var categories = _Context.Categories.ToList();
            ///var cinemas = _Context.Cinemas.ToList();
            var categories = await _CategoryRepository.GetListAsync();
            var cinemas =   await _CinemaRepository.GetListAsync();
            return View(new MovieVM()
            {
                Movie = movie,
                //Categories = categories,
                //Cinemas = cinemas,
                //MovieSubImages = _Context.MovieSubImages.Where(m=>m.MovieId == Id).ToList(),
                Categories = categories.ToList(),
                Cinemas = cinemas.ToList(),
                MovieSubImages =(await _MovieSubImageRepository.GetListAsync(m=>m.MovieId == Id)).ToList(),

            });
           
        }

        [HttpPost]
        public async Task<IActionResult> Update(Movie movie, IFormFile ImageFile, List<IFormFile> subImages)
        {
            //عشان اجيب الداتا من الداتا بيز
            //var MovieInDb = _Context.Movies.AsNoTracking().FirstOrDefault(b => b.Id == movie.Id);
            var MovieInDb = await _MovieRepository.GetOneAsync(b => b.Id == movie.Id , isTraked: false);

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
                movie.MainImg = fileName;// بعدل الاسم في الداتا بيز
                // دي عشان لما اخد فايل او صوره جديده يمسح القديم عشان المساحه
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", MovieInDb.MainImg);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            else
            {
                movie.MainImg = MovieInDb.MainImg;
            }
            ///_Context.Movies.Update(movie);
            ///_Context.SaveChanges();
                  _MovieRepository.Update(movie);
            await _MovieRepository.CommitAsync();




            ///////////////////////////  =>>>> subimages نفس الكلام الي في الكرييت زائد حبه تغييرات ////////////////////
            if (subImages != null && subImages.Count > 0)
            {
                var oldsubImages = await _MovieSubImageRepository.GetListAsync(m=>m.MovieId == movie.Id); // <==== something (was) wrong
                //delete old imgs from db
                //***** generic removerange is not declared yet  <===
                //_Context.MovieSubImages.RemoveRange(oldsubImages);
                //_Context.SaveChanges();

                 await _MovieSubImageRepository.CommitAsync();



                //Delete Subimages from wwwroot
                foreach (var item in oldsubImages)
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "MovieSubImages", item.Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                foreach (var image in subImages)
                {
                    //adding in wwwroot
                    if (image != null && image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "MovieSubImages", fileName);
                        using (var Stream = System.IO.File.Create(filePath))
                        {
                            image.CopyTo(Stream);
                        }
                        //adding in DataBase
                        await _MovieSubImageRepository.AddAsync(new MovieSubImage()
                        {
                            Image = fileName,
                            MovieId = MovieInDb.Id
                        });
                    }
                }
                //_Context.SaveChanges();
                 await _MovieRepository.CommitAsync();
            }
            return RedirectToAction(nameof(Index));
        }









        public async Task<IActionResult> Delete(int Id)
        {
            ///var movie = _Context.Movies.FirstOrDefault(c => c.Id == Id);
            var movie =await _MovieRepository.GetOneAsync(c => c.Id == Id);

            // دي عشان لما اخد فايل او صوره جديده يمسح القديم عشان المساحه
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", movie.MainImg);

            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }

            //Delete Subimages from wwwroot
            //var oldsubImages = _Context.MovieSubImages.Where(m => m.MovieId == movie.Id); 
            var oldsubImages =await _MovieSubImageRepository.GetListAsync(m => m.MovieId == movie.Id); 


            foreach (var item in oldsubImages)
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "MovieSubImages", item.Image);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            ///_Context.Movies.Remove(movie);
            ///_Context.SaveChanges();
               _MovieRepository.Delete(movie);
            await _MovieRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
