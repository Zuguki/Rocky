using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products =
                _db.Product
                    .Include(u => u.Category)
                    .Where(u => u.Category.Id == u.CategoryId)
                    .Include(p => p.ApplicationType)
                    .Where(p => p.ApplicationType.Id == p.ApplicationTypeId);
            
            return View(products);
        }

        // Get - Upsert
        public IActionResult Upsert(int? id)
        {
            var productVm = new ProductVM
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            if (id is null or 0)
                return View(productVm);

            productVm.Product = _db.Product.Find(id);

            if (productVm.Product == null)
                return NotFound();

            return View(productVm);
        }
        
        // Post - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVm)
        {
            if (ModelState.IsValid) 
            {
                var files = HttpContext.Request.Form.Files;
                var webRootPath = _webHostEnvironment.WebRootPath;

                if (productVm.Product.Id == 0)
                {
                    var upload = webRootPath + WC.ImagePath;
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        files[0].CopyTo(fileStream);
                
                    productVm.Product.Image = fileName + extension;
                    _db.Product.Add(productVm.Product);
                }
                else
                {
                    var product = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVm.Product.Id);

                    if (files.Count > 0)
                    {
                        var upload = webRootPath + WC.ImagePath;
                        var fileName = Guid.NewGuid().ToString();
                        var extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, product.Image);

                        if (System.IO.File.Exists(oldFile))
                            System.IO.File.Delete(oldFile);

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                            files[0].CopyTo(fileStream);

                        productVm.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVm.Product.Image = product.Image;
                    }

                    _db.Product.Update(productVm.Product);
                }
            
                _db.SaveChanges();
                return RedirectToAction("Index");   
            }

            productVm.CategorySelectList = _db.Category.Select(i => new SelectListItem {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            productVm.ApplicationTypeSelectList = _db.ApplicationType.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            return View(productVm);
        }

        // Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var product = _db.Product.Include(u => u.Category)
                .FirstOrDefault(u => u.Id == id);
            if (product == null)
                return NotFound();
            
            return View(product);
        }
        
        // Post - Delete
        public IActionResult DeletePost(int? id)
        {
            var product = _db.Product.Find(id);
            if (product == null)
                return NotFound();
            
            var upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, product.Image);

            if (System.IO.File.Exists(oldFile))
                System.IO.File.Delete(oldFile);

            _db.Product.Remove(product);
            _db.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}