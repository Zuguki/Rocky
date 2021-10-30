using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;

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
            var product = new Product();
            if (id == null)
                return View(product);

            product = _db.Product.Find(id);

            if (product == null)
                return NotFound();

            return View(product);
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