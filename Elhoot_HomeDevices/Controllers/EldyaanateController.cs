using Elhoot_HomeDevices.Data;
 
using Elhoot_HomeDevices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Elhoot_HomeDevices.Controllers
{
    public class EldyaanateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EldyaanateController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var deens = _context.Dayeenateys.ToList();
            decimal totalMoney = deens.Sum(m => m.Money);

            var viewModel = new EldyanaatViewmodel
            {
                 Deeens = deens,
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
        public IActionResult Create(Dayeenatey deen)
        {
            if (ModelState.IsValid)
            {
                _context.Dayeenateys.Add(deen);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }


            return View(deen);
        }

        public IActionResult Delete(int id)
        {
            var deen = _context.Dayeenateys.FirstOrDefault(m => m.Id == id);
            if (deen == null)
            {
                // Customer with the specified ID was not found, handle the error accordingly
                return NotFound();
            }

            return View(deen);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConvermid(int id)
        {
            var deen = _context.Dayeenateys.FirstOrDefault(m => m.Id == id);

            if (deen != null)
            {
                // Get the money value before deleting
                decimal moneyBeforeDeletion = deen.Money;

                // Remove the record
                _context.selecteddataDyants.RemoveRange(deen.selectedDatesRange);
                _context.Dayeenateys.Remove(deen);

                _context.SaveChanges();

                // Update the total money by subtracting the deleted record's money
                

                return RedirectToAction("Index");
            }

            return NotFound();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var deen = _context.Dayeenateys.Find(id);
            if (deen == null)
            {
                return NotFound();
            }

            return View(deen);
        }

        [HttpPost]
        public IActionResult Edit(int id, Dayeenatey editedDayeenatey)
        {
            if (id != editedDayeenatey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var originalDayeenatey = _context.Dayeenateys.Find(id);
                if (originalDayeenatey != null)
                {
                    // Get the money value before editing
                    decimal moneyBeforeEdit = originalDayeenatey.Money;

                    // Update the properties
                    originalDayeenatey.Name = editedDayeenatey.Name;
                    originalDayeenatey.Money = editedDayeenatey.Money;
                    originalDayeenatey.date = editedDayeenatey.date;
                    originalDayeenatey.Pienfits = editedDayeenatey.Pienfits;
                    originalDayeenatey.CountMonth = editedDayeenatey.CountMonth;
                    _context.SaveChanges();


                    return RedirectToAction("Index");
                }
            }

            return View(editedDayeenatey);
        }
        public IActionResult Details(int Id, string name)
        {
            ViewBag.name = name;
            var deen=_context.Dayeenateys.Include(m=>m.selectedDatesRange)
                .FirstOrDefault(m => m.Id == Id);

            if (ModelState.IsValid && deen != null)
            {
                if (deen.selectedDatesRange.Count==0)
                {

                    DateTime currendate = deen.date.AddMonths(1);
                    int? count = deen.CountMonth;
                    while (count != 0)
                    {
                        deen.selectedDatesRange.Add(new SelecteddataDyant
                        {
                            Date = currendate,
                            DeenID = deen.Id
                        });
                        currendate = currendate.AddMonths(1);
                        count--; // Use AddDays instead of AddMonths
                    }
                }

                else if (deen.selectedDatesRange.Count > deen.CountMonth)
                {
                    int gt = deen.selectedDatesRange.Count;
                    int? jk = deen.CountMonth;
                    int? count = gt - jk;


                    for (int i = 1; i <= count; i++)
                    {

                        deen.selectedDatesRange.RemoveAt(gt - i);

                    }
                }
                else
                {
                    int c = deen.selectedDatesRange.Count;
                    int? count = deen.CountMonth - c;
                    DateTime ff = deen.selectedDatesRange[c - 1].Date;
                    DateTime currendate = ff.AddMonths(1);
                    while (count != 0)
                    {
                        deen.selectedDatesRange.Add(new SelecteddataDyant
                        {
                            Date = currendate,
                            DeenID = deen.Id
                        });
                        currendate = currendate.AddMonths(1);
                        count--; // Use AddDays instead of AddMonths
                    }


                }
                _context.SaveChanges();


                var viewmodel = deen.selectedDatesRange.Select(date => new EldyanaatViewModel1
                {
                    Date = date.Date,
                    DateFree = date.DateFree,
                    IsSelected = date.IsSelected,
                    DeenID = date.DeenID,
                    Paypalce = date.Paypalce,
                    Statuse = date.Status
                }).ToList();

                var viewmodel1 = new EldyanaatViewmodel
                {
                    eldyanaatViewModel1s = viewmodel
                };

                return View("Details", viewmodel1);
            }

            return View();
        }


        public IActionResult Madunatprocess1(int DeenID, string date, bool IsCheecked = true)

        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var Deen = _context.Dayeenateys.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == DeenID);
            var selectedDateEntity = Deen.selectedDatesRange.Find(m => m.Date.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {
                selectedDateEntity.Status = "تم دفع الفايده ";
                selectedDateEntity.IsSelected = IsCheecked;
                Deen.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = Deen.Id, name = Deen.Name });
            }
            return View("Index");
        }
        public IActionResult Madunatprocess2(int DeenID, string date)

        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var deen = _context.Dayeenateys.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == DeenID);
            var selectedDateEntity = deen.selectedDatesRange.Find(m => m.Date.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {
                selectedDateEntity.Status = " ";
                selectedDateEntity.IsSelected = false;
                selectedDateEntity.DateFree = null;
                selectedDateEntity.Paypalce = " ";

                deen.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = deen.Id, name = deen.Name });
            }
            return View("Index");
        }
        public IActionResult UpdateDateFree1(int DeenID, string date, DateTime newDate)
        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var deen = _context.Dayeenateys.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == DeenID);
            var selectedDateEntity = deen.selectedDatesRange.Find(m => m.Date.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {
                selectedDateEntity.DateFree = newDate;
                deen.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = deen.Id, name = deen.Name });
            }
            return View("Index");
        }
        public IActionResult UpdateDateFree2(int DeenID, string date, string newDate)
        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var deen = _context.Dayeenateys.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == DeenID);
            var selectedDateEntity = deen.selectedDatesRange.Find(m => m.Date.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {

                selectedDateEntity.Paypalce = newDate;
                deen.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = deen.Id, name = deen.Name });
            }
            return View("Index");
        }

    }
}
