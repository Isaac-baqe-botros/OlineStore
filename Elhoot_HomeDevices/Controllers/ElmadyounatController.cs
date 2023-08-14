// ElmadyounatController.cs
using Elhoot_HomeDevices.Data;
using Elhoot_HomeDevices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;

namespace Elhoot_HomeDevices.Controllers
{
    public class ElmadyounatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ElmadyounatController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var madunaates = _context.madunaates.ToList();
            decimal totalMoney = madunaates.Sum(m => m.Money);

            var viewModel = new MadunaateViewModel
            {
                Madunaates = madunaates,
                TotalMoney = totalMoney
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Madunaate madunaate)
        {
            if (ModelState.IsValid)
            {
                _context.madunaates.Add(madunaate);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }


            return View(madunaate);
        }
        public IActionResult Delete(int id)
        {
            var cust = _context.madunaates.Find(id);
            if (cust == null)
            {
                // Customer with the specified ID was not found, handle the error accordingly
                return NotFound();
            }

            return View(cust);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConvermid(int id)
        {
            var madunaate = _context.madunaates.FirstOrDefault(m => m.Id == id);

            if (madunaate != null)
            {
                // Get the money value before deleting
                decimal moneyBeforeDeletion = madunaate.Money;

                // Remove the record
                _context.SelectedDates.RemoveRange(madunaate.selectedDatesRange);
                _context.madunaates.Remove(madunaate);
               
                _context.SaveChanges();

                // Update the total money by subtracting the deleted record's money
                var allmad = _context.madunaates.ToList();
                decimal allmoany = allmad.Select(m => m.Money).Sum();
                
                
                return RedirectToAction("Index");
            }

            return NotFound();
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var madunaate = _context.madunaates.Find(id);
            if (madunaate == null)
            {
                return NotFound();
            }

            return View(madunaate);
        }

        [HttpPost]
        public IActionResult Edit(int id, Madunaate editedMadunaate)
        {
            if (id != editedMadunaate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var originalMadunaate = _context.madunaates.Find(id);
                if (originalMadunaate != null)
                {
                    // Get the money value before editing
                    decimal moneyBeforeEdit = originalMadunaate.Money;

                    // Update the properties
                    originalMadunaate.Name = editedMadunaate.Name;
                    originalMadunaate.Money = editedMadunaate.Money;
                    originalMadunaate.date = editedMadunaate.date;
                    originalMadunaate.Pienfits = editedMadunaate.Pienfits;
                    originalMadunaate.CountMonth=editedMadunaate.CountMonth;
                
                    _context.SaveChanges();

                     
                    return RedirectToAction("Index");
                }
            }

            return View(editedMadunaate);
        }

        public IActionResult Details(int Id, string name)
        {
            ViewBag.name = name;
            var madunaate = _context.madunaates.Include(m => m.selectedDatesRange)
                .FirstOrDefault(m => m.Id == Id);

            if (ModelState.IsValid && madunaate != null)
            {
                if (madunaate.selectedDatesRange.Count == 0)
                {

                    DateTime currendate = madunaate.date.AddMonths(1);
                    int? count = madunaate.CountMonth;
                    while (count != 0)
                    {
                        madunaate.selectedDatesRange.Add(new SelectedDate
                        {
                            Date = currendate,
                            MadunatID = madunaate.Id
                        });
                        currendate = currendate.AddMonths(1);
                        count--; // Use AddDays instead of AddMonths
                    }
                }
            
                else if (madunaate.selectedDatesRange.Count> madunaate.CountMonth)
                {
                    int gt = madunaate.selectedDatesRange.Count;
                    int? jk = madunaate.CountMonth;
                    int? count = gt - jk;
                    

                    for(int i=1;i<=count;i++)
                    {
                      
                        madunaate.selectedDatesRange.RemoveAt(gt - i);
                      
                    }
                }
                else {
                   int c= madunaate.selectedDatesRange.Count;
                    int? count = madunaate.CountMonth-c;
                    DateTime ff = madunaate.selectedDatesRange[c - 1].Date;
                    DateTime currendate =ff.AddMonths(1);
                    while (count != 0)
                    {
                        madunaate.selectedDatesRange.Add(new SelectedDate
                        {
                            Date = currendate,
                            MadunatID = madunaate.Id
                        });
                        currendate = currendate.AddMonths(1);
                        count--; // Use AddDays instead of AddMonths
                    }


                }
                _context.SaveChanges();
              

                var viewmodel = madunaate.selectedDatesRange.Select(date => new MadunaatViewModel
                {
                    Date = date.Date,
                    DateFree = date.DateFree,
                    IsSelected = date.IsSelected,
                    MadunID = date.MadunatID,
                    Paypalce = date.Paypalce,
                    Statuse = date.Status
                }).ToList();

                var viewmodel1 = new MadunaateViewModel
                {
                    madunaatViewModels = viewmodel
                };

                return View("Details", viewmodel1);
            }

            return View();
        }


        public IActionResult Madunatprocess(int MadunID, string date,bool IsCheecked=true)

        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var madun = _context.madunaates.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == MadunID);
            var selectedDateEntity = madun.selectedDatesRange.Find(m => m.Date.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {
                selectedDateEntity.Status = "تم دفع الفايده ";
                selectedDateEntity.IsSelected = IsCheecked;
                madun.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details",new { Id = madun.Id, name = madun.Name });
            }
            return View("Index");
        }
        public IActionResult Madunatprocess1(int MadunID, string date)

        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var madun = _context.madunaates.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == MadunID);
            var selectedDateEntity = madun.selectedDatesRange.Find(m => m.Date.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {
                selectedDateEntity.Status = " ";
                selectedDateEntity.IsSelected = false;
                selectedDateEntity.DateFree = null;
                selectedDateEntity.Paypalce = " ";

                madun.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = madun.Id, name = madun.Name });
            }
            return View("Index");
        }
        public IActionResult UpdateDateFree(int MadunID, string date, DateTime newDate)
        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var madun = _context.madunaates.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == MadunID);
            var selectedDateEntity = madun.selectedDatesRange.Find(m => m.Date.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {
                selectedDateEntity.DateFree = newDate;
                madun.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = madun.Id, name = madun.Name });
            }
            return View("Index");
        }
        public IActionResult UpdateDateFree1(int MadunID, string date, string newDate)
        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var madun = _context.madunaates.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == MadunID);
            var selectedDateEntity = madun.selectedDatesRange.Find(m => m.Date.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {
                
                selectedDateEntity.Paypalce = newDate;
                madun.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = madun.Id, name = madun.Name });
            }
            return View("Index");
        }

    }

}
