using CenimaManagmentSystem.Data;
using CenimaManagmentSystem.Models;
using CenimaManagmentSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CenimaManagmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _Context = new ApplicationDbContext();
        private readonly IRepository<Category> _categoryRepository; // = new Repository.Repository<Category>(); 

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            //var categories = _Context.Categories.AsQueryable();
            var categories = await _categoryRepository.GetListAsync();
            return View(categories.AsEnumerable());
        }


        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create( Category category)
        {
            //_Context.Categories.Add(category);
            //_Context.SaveChanges();

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Update( int Id )
        {
            //var category = _Context.Categories.FirstOrDefault(c=>c.Id == Id);
              var category = await _categoryRepository.GetOneAsync(c=>c.Id == Id);
            if (category == null) 
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            //_Context.Categories.Update(category);
            //_Context.SaveChanges();
                  _categoryRepository.Update(category);
            await _categoryRepository.CommitAsync();            


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            //var categoty = _Context.Categories.FirstOrDefault(c => c.Id == id);
              var categoty =await _categoryRepository.GetOneAsync(c => c.Id == id);
            //_Context.Categories.Remove(categoty);
            //_Context.SaveChanges();

                  _categoryRepository.Delete(categoty);
            await _categoryRepository.CommitAsync();

            return RedirectToAction (nameof(Index));
        }

    }
}
