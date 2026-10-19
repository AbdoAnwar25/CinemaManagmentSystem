using CenimaManagmentSystem.Data;
using CenimaManagmentSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CenimaManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _Context = new ApplicationDbContext();

        public IActionResult Index()
        {
            var categories = _Context.Categories.AsQueryable();

            return View(categories.AsEnumerable());
        }


        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create( Category category)
        {
            _Context.Categories.Add(category);
            _Context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public IActionResult Update( int Id )
        {
            var category = _Context.Categories.FirstOrDefault(c=>c.Id == Id);
            if (category == null) 
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            _Context.Categories.Update(category);
            _Context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var categoty = _Context.Categories.FirstOrDefault(c => c.Id == id);
            _Context.Categories.Remove(categoty);
            _Context.SaveChanges();

            return RedirectToAction (nameof(Index));
        }

    }
}
