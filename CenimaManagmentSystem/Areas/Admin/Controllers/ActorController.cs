using CenimaManagmentSystem.Data;
using CenimaManagmentSystem.Models;
using CenimaManagmentSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CenimaManagmentSystem.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class ActorController : Controller
    {

        /// private readonly ApplicationDbContext _Context = new ApplicationDbContext();
        private readonly IRepository<Actor> _ActorRepository; // = new Repository<Actor>();

        public ActorController(IRepository<Actor> actorRepository)
        {
            _ActorRepository = actorRepository;
        }



        //اندكس التطبيق
        //public IActionResult Index()
        //{

        //    var actors = _Context.Actors.AsQueryable();

        //    return View(actors.AsEnumerable());
        //}


        // اندكس التاسك الي فيها فيلتر ب الديت و الباجينيشن
        public async Task<IActionResult> Index( int pageNumber = 1)
        {
            int pageSize = 5; // عدد العناصر في كل صفحة (ممكن تغيرها براحتك)
            ///var actors = _Context.Actors.AsQueryable();
            var actors = await _ActorRepository.GetListAsync();

            // 1. لو في بحث بالتاريخ
         

            // حساب إجمالي عدد العناصر عشان نعرف هنعمل كم صفحة (لو احتجناها)
            int totalItems = actors.Count();

            // 2. تطبيق الـ Pagination (تقسيم الصفحات)
            var pagedActors = actors
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            // بنبعت رقم الصفحة الحالية وإجمالي الصفحات للـ View عشان أزرار التنقل
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(pagedActors);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Actor actor, IFormFile ImageFile) // الشرح تحت 
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "-" + ImageFile.FileName;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var Stream = System.IO.File.Create(filePath))
                {
                    ImageFile.CopyTo(Stream);
                }

                actor.Logo = fileName;

            }
            ///_Context.Actors.Add(actor);
            ///_Context.SaveChanges();
            await _ActorRepository.AddAsync(actor);
            await _ActorRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            var actor = await _ActorRepository.GetOneAsync(c => c.Id == Id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Actor actor, IFormFile ImageFile)
        {
            //عشان اجيب الداتا من الداتا بيز
            ///var ActorInDb = _Context.Actors.AsNoTracking().FirstOrDefault(b => b.Id == actor.Id);
               var ActorInDb = await _ActorRepository.GetOneAsync(b => b.Id == actor.Id , isTraked : false);

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

                actor.Logo = fileName;// بعدل الاسم في الداتا بيز

                // دي عشان لما اخد فايل او صوره جديده يمسح القديم عشان المساحه
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", ActorInDb.Logo);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            else
            {
                actor.Logo = ActorInDb.Logo;
            }


            ///_Context.Actors.Update(actor);
            ///_Context.SaveChanges();
                  _ActorRepository.Update(actor);
            await _ActorRepository.CommitAsync();
            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> Delete(int Id)
        {
            var actor =await _ActorRepository.GetOneAsync(c => c.Id == Id);

            // دي عشان لما اخد فايل او صوره جديده يمسح القديم عشان المساحه
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", actor.Logo);

            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }

            //_Context.Actors.Remove(actor);
            //_Context.SaveChanges();
            _ActorRepository.Delete(actor);
            await _ActorRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }






        ///////////////////////////////////////////////////////////التاسك بتاعه الداونلود اكشن
        public IActionResult DownloadFile(string fileName)
        {
            // لو اسم الملف فاضي أو مش موجود نرجع Not Found
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound("File name is not specified.");
            }

            // بنجمع مسار الفلدر مع اسم الملف عشان نجيب المسار الكامل على السيرفر
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            // نتاكد إن الملف فعلاً موجود على الهارد ديسك
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found on server.");
            }

            // نقرأ الملف ونرجعه للمتصفح كـ FileResult عشان يبدأ يحمله تلقائياً
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }







    }
}
