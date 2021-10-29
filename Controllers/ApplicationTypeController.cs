using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            IEnumerable<ApplicationType> applicationTypes = _db.ApplicationType;
            
            return View(applicationTypes);
        }

        // Get - Create
        public IActionResult Create()
        {
            return View();
        }
        
        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType applicationType)
        {
            if (!ModelState.IsValid) return NotFound();
            
            _db.ApplicationType.Add(applicationType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        // Get - Edit
        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
                return NotFound();
            
            var applicationType = _db.ApplicationType.Find(id);
            if (applicationType == null)
                return NotFound();

            return View(applicationType);
        }
        
        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType applicationType)
        {
            _db.ApplicationType.Add(applicationType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        // Get - Delete
        public IActionResult Delete(int? id)
        {
            return View();
        }
        
        //Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            _db.ApplicationType.Add();
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}