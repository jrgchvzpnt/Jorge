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
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _db.Catagories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Categoría creada exitosamente";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Category? categoryFromdb = _db.Catagories.Find(id);
            //Category? categoryFromdb1 = _db.Catagories.FirstOrDefault(c => c.Id == id);
            //Category? categoryFromdb2 = _db.Catagories.Where(c => c.Id == id).FirstOrDefault();

            if (categoryFromdb == null)
            {
                return NotFound();
            }
            return View(categoryFromdb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Catagories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Categoría actualizada exitosamente";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Category? categoryFromdb = _db.Catagories.Find(id);
           

            if (categoryFromdb == null)
            {
                return NotFound();
            }
            return View(categoryFromdb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Catagories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Catagories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Categoría eliminada exitosamente";
            return RedirectToAction("Index");
        }
    }
}
