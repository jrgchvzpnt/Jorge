using JorgeBook.DataAccess.Data;
using JorgeBook.DataAccess.Repository.IRepository;
using JorgeBook.Models;
using JorgeBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.IdentityModel.Tokens;

namespace JorgeBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

            //Projections in EF Core
       
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
    
            ProductVM productoVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productoVM);
            }
            else
            {
                // Update
                productoVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productoVM);
            }
           
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM , IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                // posicionarse en el wwwroot del proyecto
                string wwwRoot = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRoot, @"images\product");
                    using (var filestream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(filestream);    
                    }

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRoot, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);    
                        }
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0) 
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                
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
        
        #region llamado de APIs
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { Data = objProductList});

        }

        [HttpDelete]
        public IActionResult delete(int? id)
        {

            var productTobeDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (productTobeDelete == null)
            {
                return Json(new { success = false, message = "Error al Eliminar" });

            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productTobeDelete.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productTobeDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Correctamente" });

        
        }
        #endregion
    }
}
