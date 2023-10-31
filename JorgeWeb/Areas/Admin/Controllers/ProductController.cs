using JorgeBook.DataAccess.Data;
using JorgeBook.DataAccess.Repository.IRepository;
using JorgeBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace JorgeBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Producto creada exitosamente";
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
            Product? productFromdb = _unitOfWork.Product.Get(u => u.Id == id);
            //Category? categoryFromdb1 = _db.Catagories.FirstOrDefault(c => c.Id == id);
            //Category? categoryFromdb2 = _db.Catagories.Where(c => c.Id == id).FirstOrDefault();

            if (productFromdb == null)
            {
                return NotFound();
            }
            return View(productFromdb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Producto actualizada exitosamente";
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
            Product? categoryFromdb = _unitOfWork.Product.Get(u => u.Id == id);


            if (categoryFromdb == null)
            {
                return NotFound();
            }
            return View(categoryFromdb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Producto eliminada exitosamente";
            return RedirectToAction("Index");
        }
    }
}
