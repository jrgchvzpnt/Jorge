using JorgeWeb.Data;
using JorgeWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace JorgeWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category>  objCategoryList = _db.Catagories.ToList();  
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if(ModelState.IsValid)
            {
                _db.Catagories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
    }
}
