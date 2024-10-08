using Elhoot_HomeDevices.Data;
using Elhoot_HomeDevices.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Elhoot_HomeDevices.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context) {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {

            var result = _context.Categories.ToList();

            decimal totla =+ result.Sum(c => c.Totalprice);
            decimal totalAllCategoriesPrice = _context.Products.Sum(p => p.Price * p.Count);
            ViewBag.totalprice = totalAllCategoriesPrice;
            return View(result);
        }
        public IActionResult Creat ()
        {
            //_context.Categories.Add(category);
            //return RedirectToAction("Index");
            return View();
        }
        [HttpPost]
       
        public async Task<IActionResult> Creat(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");   
        }
        public IActionResult Delete()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Handle category not found case here

            return RedirectToAction("Delete");
        }

        public IActionResult Update(int ID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item=_context.Categories.FirstOrDefault(item=>item.Id == ID);
            return View(item);
        }
        [HttpPost]
        public IActionResult Update(Category category )
        {
            if (ModelState.IsValid)
            {
                 _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Details(int Id)
        {

            var cat = _context.Categories.Find(Id);
            if (ModelState.IsValid)
            {

                if (cat == null)
                {
                    return NotFound();
                }

                decimal totalprice = _context.Products.Where(p => p.CategoryId == Id).Sum(p => p.Price);
                var prodct = _context.Products.Where(p => p.CategoryId == Id).ToList();

                var ViewModel = new CategoryWithProductsViewModel
                {
                    category = cat,
                    products = prodct
                };
                return View(ViewModel);
            }
            return View();
           
        }
    }
}
