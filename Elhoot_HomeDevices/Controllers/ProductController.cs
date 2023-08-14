using Elhoot_HomeDevices.Data;
using Elhoot_HomeDevices.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Globalization;
namespace Elhoot_HomeDevices.Controllers
{
    
    public class ProductController : Controller
    {
 
      


        // GET: ProductController
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
  
        public ActionResult Index(int categoryId,string CatName)
        {
            TempData["CategoryId"] = categoryId;
            var cat =_context.Categories.Find(categoryId);
            TempData["name"] = CatName;
            var Prods = _context.Products.Where(p => p.CategoryId == categoryId).OrderBy(p=>p.Name);
             
            decimal totalPrice = Prods.Sum(p => p.Price * p.Count);
            cat.Totalprice=totalPrice;
            ViewBag.TotalPrice = totalPrice;
            return View(Prods);
        }

        // GET: ProductController/Details/5=
        public ActionResult Details(int id)
        {

            var prod=_context.Products.FirstOrDefault(p=>p.Id==id);
            var detl = new DetalisForProduct
            {
                Name = prod.Name,
                totalprice = prod.Price * prod.Count,
                count = prod.Count

            };
            return View(detl);
        }

        // GET: ProductController/Create
        //public ActionResult Create()
        //{
        //    var cat = _context.Categories.Find(TempData["CategoryId"]);
        //    var product = new Product
        //    {
        //        CategoryId = (int)TempData["CategoryId"],

        //        Category = cat
        //    };
        //    return View(product);
        //}
        public ActionResult CreateForCategory( string Catname)
        {
            ViewBag.name= Catname;   
            var cat = _context.Categories.Find(TempData["CategoryId"]);
            if (cat == null)
            {
                return NotFound();
            }
            var product = new Product
            {
                CategoryId = (int)TempData["CategoryId"],
                Category = cat
            };
            return View(product);
        }


        // POST: ProductController/Create
        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateForCategory(Product product)

        {
            var cat = _context.Categories.Find(product.CategoryId);
            if (ModelState.IsValid)
            {
                product.TotalPriceCount = product.Count * product.Price;

                // Save changes to the database
                _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Product", new { categoryId = product.CategoryId, CatName =cat.Name });
                        
                
            }
            
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    // Log or debug the error message
                    Debug.WriteLine(error.ErrorMessage);
                }
            }

            return View("Create",product);
        }



        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Catogeryname = _context.Categories.ToList();
            var pro = _context.Products.FirstOrDefault(or => or.Id == id);
            return View(pro);

        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product prod)
        {
            ViewBag.Catogeryname = _context.Categories.ToList();
            var cat = _context.Categories.Find(prod.CategoryId);
            try
            {
                var pro = _context.Products.FirstOrDefault(or => or.Id == id);
                if (pro != null)
                {   pro.Price = prod.Price;
                    pro.Count = prod.Count;
                    pro.CategoryId = prod.CategoryId;
                    pro.Count = prod.Count;
                    pro.Name= prod.Name;
                    pro.TotalPriceCount = prod.Count * prod.Price;
                 
                  
                    
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index), new { categoryId =pro.CategoryId, CatName =cat.Name});
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5


        // POST: ProductController/Delete/5


        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var product = _context.Products.Find(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Products.Remove(product);
        //    _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}















        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]


        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var prod = _context.Products.FirstOrDefault(pro => pro.Id == id);
                _context.Products.Remove(prod);
                var category = _context.Categories.Find(prod.CategoryId);
                if (category != null)
                {
                    //  category.Count--;
                    _context.Entry(category).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index), new { categoryId = prod.CategoryId, CatName = category.Name });
            }
            catch
            {
                return View();
            }
        }
    }
}
