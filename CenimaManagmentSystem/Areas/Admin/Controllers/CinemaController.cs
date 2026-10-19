using CenimaManagmentSystem.Data;
using CenimaManagmentSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CenimaManagmentSystem.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly ApplicationDbContext _Context = new ApplicationDbContext();

        //اندكس التطبيق
        //public IActionResult Index()
        //{

        //    var cinemas = _Context.Cinemas.AsQueryable();

        //    return View(cinemas.AsEnumerable());
        //}


        // اندكس التاسك الي فيها فيلتر ب الديت و الباجينيشن
        public IActionResult Index(int pageNumber = 1)
        {
            int pageSize = 5; // عدد العناصر في كل صفحة (ممكن تغيرها براحتك)
            var cinemas = _Context.Cinemas.AsQueryable();

            // 1. لو في بحث بالتاريخ


            // حساب إجمالي عدد العناصر عشان نعرف هنعمل كم صفحة (لو احتجناها)
            int totalItems = cinemas.Count();

            // 2. تطبيق الـ Pagination (تقسيم الصفحات)
            var pagedCinemas = cinemas
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            // بنبعت رقم الصفحة الحالية وإجمالي الصفحات للـ View عشان أزرار التنقل
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(pagedCinemas);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile ImageFile) // الشرح تحت 
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + ImageFile.FileName;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var Stream = System.IO.File.Create(filePath))
                {
                    ImageFile.CopyTo(Stream);
                }

                cinema.Logo = fileName;

            }
            _Context.Cinemas.Add(cinema);
            _Context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Update(int Id)
        {
            var cinema = _Context.Cinemas.FirstOrDefault(c => c.Id == Id);
            if (cinema == null)
            {
                return NotFound();
            }
            return View(cinema);
        }

        [HttpPost]
        public IActionResult Update(Cinema cinema, IFormFile ImageFile)
        {
            //عشان اجيب الداتا من الداتا بيز
            var CinemaInDb = _Context.Cinemas.AsNoTracking().FirstOrDefault(b => b.Id == cinema.Id);

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

                cinema.Logo = fileName;// بعدل الاسم في الداتا بيز

                // دي عشان لما اخد فايل او صوره جديده يمسح القديم عشان المساحه
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", CinemaInDb.Logo);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            else
            {
                cinema.Logo = CinemaInDb.Logo;
            }


            _Context.Cinemas.Update(cinema);
            _Context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }





        public IActionResult Delete(int Id)
        {
            var cinema = _Context.Cinemas.FirstOrDefault(c => c.Id == Id);

            // دي عشان لما اخد فايل او صوره جديده يمسح القديم عشان المساحه
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", cinema.Logo);

            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }

            _Context.Cinemas.Remove(cinema);
            _Context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
