using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
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
            IEnumerable<Category> categories = _db.Category;
            return View(categories);
        }

        // Get - Create
        public IActionResult Create()
        {
            return View();
        }
        
        // Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");   
            }

            return View(category);
        }
        
        // Get - Edit
        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var category = _db.Category.Find(id);
            
            if (category == null)
                return NotFound();
            
            return View(category);
        }
        
        // Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid) return View(category);
            
            _db.Category.Update(category); // TODO: Make update without swap
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        // Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var category = _db.Category.Find(id);
            
            if (category == null)
                return NotFound();
            
            return View(category);
        }
        
        // Get - Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var category = _db.Category.Find(id);
            if (category == null)
                return NotFound();
            
            _db.Category.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}