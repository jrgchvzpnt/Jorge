using JorgeBook.DataAccess.Data;
using JorgeBook.DataAccess.Repository.IRepository;
using JorgeBook.Models;
using JorgeBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

            //Projections in EF Core
       
            return View(objProductList);
        }
        public IActionResult Create()
        {
        //    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        //    {
        //        Text = u.Name,
        //        Value = u.Id.ToString()
        //    });

            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM productoVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            return View(productoVM);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Producto creada exitosamente";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

                return View(productVM);
            }
         
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
