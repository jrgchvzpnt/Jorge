using Jorge.DataAccess.Data;
using Jorge.DataAccess.Repository.IRepository;
using Jorge.Models;
using Microsoft.AspNetCore.Mvc;

namespace JorgeWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }
        public IActionResult Index()
        {
            List<Category>  objCategoryList = _categoryRepo.GetAll().ToList();  
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
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
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
            Category? categoryFromdb = _categoryRepo.Get(u => u.Id == id);
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
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
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
            Category? categoryFromdb = _categoryRepo.Get(u => u.Id == id);


            if (categoryFromdb == null)
            {
                return NotFound();
            }
            return View(categoryFromdb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _categoryRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _categoryRepo.Remove(obj);
            _categoryRepo.Save();
            TempData["success"] = "Categoría eliminada exitosamente";
            return RedirectToAction("Index");
        }
    }
}
