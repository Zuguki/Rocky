using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _db.Product;

            foreach (var product in products)
                product.Category = _db.Category.FirstOrDefault(c => c.Id == product.Id);

            return View(products);
        }

        // Get - Upsert
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> categoryDropDown = _db.Category.Select(i => new SelectListItem {
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //ViewBag.CategoryDropDown = categoryDropDown;

            //var product = new Product();

            var productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
                return View(productVM);

            productVM.Product = _db.Product.Find(id);

            if (productVM.Product == null)
                return NotFound();

            return View(productVM);
        }
        
        // Post - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Product.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");   
            }

            return View(product);
        }
        
        // Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var product = _db.Product.Find(id);
            
            if (product == null)
                return NotFound();
            
            return View(product);
        }
        
        // Get - Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = _db.Product.Find(id);
            if (product == null)
                return NotFound();
            
            _db.Product.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}