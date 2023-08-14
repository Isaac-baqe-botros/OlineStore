using Elhoot_HomeDevices.Data;
using Elhoot_HomeDevices.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Elhoot_HomeDevices.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? customerId, string? customername)
        {
            TempData["CustomerId"] = customerId;
            TempData["customername"] = customername;
             
             
            var order = _context.Orders
                .Include(o => o.DatesInRange)
                .FirstOrDefault(o => o.CustomerId == customerId);

            if (order != null)
            {
                var orderViewModels = order.DatesInRange.Select(date => new OrderViewModel
                {
                    OrderId = order.Id,
                    Date = date.Date,
                    IsSelected = date.IsSelected,
                    Statuse=date.Statuse,
                    DateFree=date.DateFree,
                    Paypalce=date.Paypalce
                    // You can set this based on some criteria if needed
                    // Add other properties as needed for display
                }).ToList();
               
                var viewModel = new OrderDetailsViewModel
                { 
                    Order = order,
                    OrderViewModels = orderViewModels
                };

                return View(viewModel); // Display the regular view with order details
            }
            else
            {
                return View("EmptyOrderIndex"); // Retu the empty view if no order exists
            }
        }


        [HttpPost]

        [HttpPost]
        [HttpPost]


        //private decimal CalculatePriceAfterBenfits(Order order)
        //{

        //    decimal price = order.PriceAfterBenfits;
        //    decimal pricprermonth = price / order.Peroid;
        //    decimal res = price - pricprermonth;

        //    order.PriceAfterBenfits = res;
        //    _context.SaveChanges();
        //    foreach (var date in order.DatesInRange)
        //    {
        //        if (date.IsSelected)
        //        {
        //            totalBenfits += (restPrice * order.Penfits / 100) * order.Peroid;
        //        }
        //    }

        //    return res;
        //}
        [HttpGet]
        public IActionResult Create(string? customername)
        {
            ViewBag.CustomerName = customername;
            var cust = _context.Customers.Find(TempData["CustomerId"]);
            if (cust == null)
            {
                return NotFound();
            }


            var order = new Order
            {
                CustomerId = (int)TempData["CustomerId"],
                Customer = cust
            };
            return View(order);
        }

        [HttpPost]
        public IActionResult CreatOrder(Order order)
        {
            var custm = _context.Customers.Find(order.CustomerId);

            if (ModelState.IsValid)
            {

                order.RestPrice = (order.AllPrice) - order.PayedPrice;
                order.PriceAfterBenfits = (((order.RestPrice * order.Penfits / 100) * (order.Peroid)) + (order.RestPrice));
                order.Enddate = order.Startdate.AddMonths(order.Peroid-1);
                order.PriceAfterpermonth = order.PriceAfterBenfits / order.Peroid;

                DateTime currendate = order.Startdate;
                while (currendate <= order.Enddate)
                {
                    order.DatesInRange.Add(new StoreDate
                    {
                        Date = currendate,
                        OrderId = order.Id, 
                        Statuse=""// Use order.Id as the OrderId
                    });
                    currendate = currendate.AddMonths(1); // Use AddDays instead of AddMonths
                }
                //foreach (var date in order.DatesInRange)
                //{
                //    if (date.IsSelected)
                //    {
                //        var selectedDate = new SelectedDate
                //        {
                //            OrderId = order.Id,
                //            Date = date.Date
                //        };
                //        _context.SelectedDates.Add(selectedDate);
                //    }
                //}

                _context.Orders.Add(order);
                _context.SaveChanges();
                return RedirectToAction("Index", "Order", new { customerId = order.CustomerId, customername = custm.Name });
            }
            return View(order);
        }

        public IActionResult Edit(int id)
        {
            var order = _context.Orders.Find(id);
            return View(order);
        }
        [HttpPost]
        public IActionResult Edit(Order order, int id)
        {
            var ord = _context.Orders.Include(o => o.DatesInRange).FirstOrDefault(o => o.Id == id);
            var cust = _context.Customers.Find(ord.CustomerId);

            if (ModelState.IsValid)
            {
                // Update order properties
                ord.productNmae = order.productNmae;
                ord.PayedPrice = order.PayedPrice;
                ord.Penfits = order.Penfits;
                ord.Comment = order.Comment;
                ord.AllPrice = order.AllPrice;
                ord.Peroid = order.Peroid;
                ord.Startdate = order.Startdate;
                ord.Enddate = order.Startdate.AddMonths(ord.Peroid - 1);
                ord.RestPrice = order.AllPrice - order.PayedPrice;
                ord.PriceAfterBenfits = (((ord.RestPrice * ord.Penfits / 100) * (ord.Peroid)) + (ord.RestPrice));
                ord.PayerTime = order.PayerTime;
                ord.PriceAfterpermonth = ord.PriceAfterBenfits / order.Peroid;

                // Remove old dates from DatesInRange
                _context.RemoveRange(ord.DatesInRange);

                // Add new dates to DatesInRange based on updated Startdate and Peroid
                DateTime currentDate = ord.Startdate;
                while (currentDate <= ord.Enddate)
                {
                    ord.DatesInRange.Add(new StoreDate
                    {
                        Date = currentDate,
                        OrderId = ord.Id,
                        Statuse = ""
                        
                    });

                    currentDate = currentDate.AddMonths(1);
                }

                // Save the changes to the database
                _context.SaveChanges();

                return RedirectToAction("index", new { customerId = ord.CustomerId, customername = cust.Name });
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            var deen = _context.Orders.FirstOrDefault(m => m.Id == id);
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
            var order = _context.Orders.Include(o => o.DatesInRange).FirstOrDefault(o => o.Id == id);

            var cust = _context.Customers.Find(order.CustomerId);
            if (ModelState.IsValid)
            {
                _context.storeDates.RemoveRange(order.DatesInRange);
                _context.Orders.Remove(order);;
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Customer");
        }
        [HttpPost]
        [HttpPost]
        public IActionResult UpdateDateFree(int orderId, string date, DateTime newDate)
        {
            // Parse the date string back to a DateTime object
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Get the order from the database
            var order = _context.Orders.Include(o => o.DatesInRange).FirstOrDefault(o => o.Id == orderId);

            // Find the corresponding date in the DatesInRange list
            var selectedDateEntity = order.DatesInRange.FirstOrDefault(d => d.Date.Date == selectedDate.Date);

            if (selectedDateEntity != null)
            {
                // Update the DateFree property of the selected date entity
                selectedDateEntity.DateFree = newDate;

                // Save the changes to the database
                _context.SaveChanges();

                return Ok(); // Return a success status
            }

            return NotFound(); // Return a not found status if the date is not found
        }
        public IActionResult UpdateDateFree1(int orderId, string date, string newDate)
        {
            // Parse the date string back to a DateTime object
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Get the order from the database
            var order = _context.Orders.Include(o => o.DatesInRange).FirstOrDefault(o => o.Id == orderId);

            // Find the corresponding date in the DatesInRange list
            var selectedDateEntity = order.DatesInRange.FirstOrDefault(d => d.Date.Date == selectedDate.Date);

            if (selectedDateEntity != null)
            {
                // Update the DateFree property of the selected date entity
                selectedDateEntity.Paypalce = newDate;

                // Save the changes to the database
                _context.SaveChanges();

                return Ok(); // Return a success status
            }

            return NotFound(); // Return a not found status if the date is not found
        }

    }
}
