using Elhoot_HomeDevices.Data;
using Elhoot_HomeDevices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace Elhoot_HomeDevices.Controllers
{
    public class ElsaaahpController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ElsaaahpController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (ModelState.IsValid)
            {
                var Elsaa = _context.Elsaahps.ToList();
                var NewViewModiel = Elsaa.Select(elsaa => new ElsaahpViewModelIndex
                {
                    Id = elsaa.Id,
                    clientName = elsaa.ClientName,
                    Productname = elsaa.ProductName,
                    Allpeice = elsaa.Allprice,
                    CreatedDate = elsaa.Date,
                    CountMouth = elsaa.CountMounth
                }).ToList();
                ViewBag.money = _context.Elsaahps.Sum(e => e.Allprice);
                // Pass the NewViewModiel (ElsaahpViewModelIndex) to the view
                return View("Index", NewViewModiel);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Elsaahp  elsaahp)
        {
            if (ModelState.IsValid)
            {
                 
                _context.Elsaahps.Add(elsaahp);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }


            return View(elsaahp);
        }
        public IActionResult Delete(int id)
        {
            var cust = _context.Elsaahps.Find(id);
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
            var Elsaa = _context.Elsaahps.FirstOrDefault(m => m.Id == id);

            if (Elsaa != null)
            {
                // Get the money value before deleting
                

                // Remove the record
                //_context.SelectedDates.RemoveRange(ELsaa.selectedDatesRange);
                _context.Elsaahps.Remove(Elsaa);

                _context.SaveChanges();

                // Update the total money by subtracting the deleted record's money
             

                return RedirectToAction("Index");
            }

            return NotFound();
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Elsaa = _context.Elsaahps.Find(id);
            if (Elsaa == null)
            {
                return NotFound();
            }

            return View(Elsaa);
        }

        [HttpPost]
        public IActionResult Edit(int id, Elsaahp editedElsaahp)
        {
            if (id != editedElsaahp.Id)
            {
                return NotFound();
            }
            var originalElsaap = _context.Elsaahps.Find(id);
            if (ModelState.IsValid)
            {
              
                if (originalElsaap != null)
                {
                    // Get the money value before editing



                    //// Update the properties
                    originalElsaap.ClientName = editedElsaahp.ClientName;
                    originalElsaap.ProductName = editedElsaahp.ProductName;
                    originalElsaap.Allprice = editedElsaahp.Allprice;
                    originalElsaap.Date = editedElsaahp.Date;
                    originalElsaap.CountMounth = editedElsaahp.CountMounth;

                    _context.SaveChanges();


                    return RedirectToAction("Index");
                }
            }

            return View(editedElsaahp);
        }

        public IActionResult Details(int Id, string name,decimal price)
        {
            ViewBag.name = name;
            ViewBag.price = price;
            var ELsaa = _context.Elsaahps.Include(m => m.selectedDatesRange)
                .FirstOrDefault(m => m.Id == Id);

            if (ModelState.IsValid && ELsaa != null)
            {
                if (ELsaa.selectedDatesRange.Count == 0)
                {

                    DateTime currendate = ELsaa.Date.AddMonths(1);
                    int? count = ELsaa.CountMounth;
                    while (count != 0)
                    {
                        ELsaa.selectedDatesRange.Add(new SelectedDateElsaahp 
                        {
                            Dateofsahp = currendate,
                            ElsahpID = ELsaa.Id
                        });
                        currendate = currendate.AddMonths(1);
                        count--; // Use AddDays instead of AddMonths
                    }
                }

                else if (ELsaa.selectedDatesRange.Count > ELsaa.CountMounth)
                {
                    int gt = ELsaa.selectedDatesRange.Count;
                    int? jk =   ELsaa.CountMounth;
                    int? count = gt - jk;


                    for (int i = 1; i <= count; i++)
                    {

                        ELsaa.selectedDatesRange.RemoveAt(gt - i);

                    }
                }
                else
                {
                    int c = ELsaa.selectedDatesRange.Count;
                    int? count = ELsaa.CountMounth - c;
                    DateTime ff = ELsaa.selectedDatesRange[c - 1].Dateofsahp;
                    DateTime currendate = ff.AddMonths(1);
                    while (count != 0)
                    {
                        ELsaa.selectedDatesRange.Add(new SelectedDateElsaahp
                        {
                            Dateofsahp = currendate,
                            ElsahpID = ELsaa.Id
                        });
                        currendate = currendate.AddMonths(1);
                        count--; // Use AddDays instead of AddMonths
                    }


                }
                _context.SaveChanges();


                var viewmodel = ELsaa.selectedDatesRange.Select(date => new ElsaahpViewModel
                {
                    Date = date.Dateofsahp,
                    DateFree = date.DateFree,
                    IsSelected = date.IsSelected,
                    ElsahpID = date.ElsahpID,
                    Paypalce = date.Paypalce,
                    PayPrice=date.PayPrice,
                    Statuse = date.Status
                }).ToList();

                var viewmodel1 = new ElsaapViewModelCollectio
                {
                    elsaahpViewModels = viewmodel
                };
                _context.SaveChanges();
                return View("Details", viewmodel1);
            }

            return View();
        }



        public IActionResult Madunatprocess1(int DeenID, string date, bool IsCheecked = true)

        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var Deen = _context.Elsaahps.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == DeenID);
            var selectedDateEntity = Deen.selectedDatesRange.Find(m => m.Dateofsahp.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {
                selectedDateEntity.Status = "تم الدفع   ";
                selectedDateEntity.IsSelected = IsCheecked;
                Deen.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = Deen.Id, name = Deen.ClientName ,price=Deen.Allprice });
            }
            return View("Index");
        }
        public IActionResult Madunatprocess2(int DeenID, string date)

        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var deen = _context.Elsaahps.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == DeenID);
            var selectedDateEntity = deen.selectedDatesRange.Find(m => m.Dateofsahp.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {
                selectedDateEntity.Status = " ";
                selectedDateEntity.IsSelected = false;
                selectedDateEntity.DateFree = null;
                selectedDateEntity.Paypalce = " ";
                  
                deen.selectedDatesRange.Add(selectedDateEntity);
                deen.Allprice += selectedDateEntity.PayPrice;
                selectedDateEntity.PayPrice = 0;
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = deen.Id, name = deen.ClientName ,price=deen.Allprice });
            }
            return View("Index");
        }
        public IActionResult UpdateDateFree1(int DeenID, string date, DateTime newDate)
        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var deen = _context.Elsaahps.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == DeenID);
            var selectedDateEntity = deen.selectedDatesRange.Find(m => m.Dateofsahp.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {
                selectedDateEntity.DateFree = newDate;
                deen.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = deen.Id, name = deen.ClientName ,price=deen.Allprice});
            }
            return View("Index");
        }
        public IActionResult UpdateDateFree2(int DeenID, string date, string newDate)
        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var deen = _context.Elsaahps.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == DeenID);
            var selectedDateEntity = deen.selectedDatesRange.Find(m => m.Dateofsahp.Date == selectedDate.Date);
            if (selectedDateEntity != null)
            {

                selectedDateEntity.Paypalce = newDate;
                deen.selectedDatesRange.Add(selectedDateEntity);
                _context.SaveChanges();
                return RedirectToAction("Details", new { Id = deen.Id, name = deen.ClientName });
            }
            return View("Index");
        }
        public IActionResult UpdateDateFree3(int DeenID, string date, decimal newDate)
        {
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var deen = _context.Elsaahps.Include(m => m.selectedDatesRange).FirstOrDefault(m => m.Id == DeenID);
            var selectedDateEntity = deen.selectedDatesRange.Find(m => m.Dateofsahp.Date == selectedDate.Date);

            if (selectedDateEntity != null)
            {
                selectedDateEntity.PayPrice = newDate;
                deen.Allprice = CalculatePriceBenfits(newDate, deen);
                _context.SaveChanges();

                return RedirectToAction("Details", new { Id = deen.Id, name = deen.ClientName, price = deen.Allprice });
            }

            return View("Index");
        }

        private decimal CalculatePriceBenfits(decimal price, Elsaahp deen)
        {
            decimal result = deen.Allprice - price;
            return result;
        }


       
    }
}
