using Elhoot_HomeDevices.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Elhoot_HomeDevices.Controllers
{
    public class PayElqestController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PayElqestController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Pay(int orderid)
        {
            var ord =_context.Orders.Find(orderid);
            
            return View();
        }
        public IActionResult UpdatePriceAfterBenfits(int orderid, string date, bool isChecked=true)
        {
            // Parse the date string back to a DateTime object
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Get the order from the database
            var order = _context.Orders.Include(o => o.DatesInRange).FirstOrDefault(o => o.Id == orderid);
            var custm = _context.Customers.FirstOrDefault(c => c.Id == order.CustomerId);

            // Find the corresponding date in the DatesInRange list
            var selectedDateEntity = order.DatesInRange.FirstOrDefault(d => d.Date.Date == selectedDate.Date);

            // Check if the date already exists in the DatesInRange list
            if (selectedDateEntity != null)
            {


                selectedDateEntity.IsSelected = isChecked;

                selectedDateEntity.Statuse =$"تم الدفع { order.PriceAfterpermonth} ";


                order.DatesInRange.Add(selectedDateEntity);
                // Sort the DatesInRange list based on the Date property and get the selected dates
                //  var selectedDates = order.DatesInRange.Where(d => d.IsSelected).OrderBy(d => d.Date).ToList();

                // Calculate the PriceAfterBenfits based on the selected dates
                //TempData["datedeleted"] = selectedDateEntity.Date.ToString();
                //if (selectedDateEntity != null)
                //{
                //    order.DatesInRange.Remove(selectedDateEntity);
                //}

                order.PriceAfterBenfits = CalculatePriceAfterBenfits(order);

                // Save the changes to the database
                _context.SaveChanges();

                // Return the updated PriceAfterBenfits value
                return RedirectToAction("Index","Order", new { customerId = order.CustomerId, customername = custm.Name, dateTime = selectedDateEntity.Date });
            }

            return RedirectToAction("Index", "Customer");
        }

        private decimal CalculatePriceAfterBenfits(Order order)
        {
            decimal pricePinft = order.PriceAfterBenfits;
            decimal pricpinfmonth = order.PriceAfterpermonth;



            // Calculate the PriceAfterBenfits by adding the total benefits to the rest price
            decimal priceAfterBenfits1 = pricePinft - pricpinfmonth;
            order.PriceAfterBenfits = priceAfterBenfits1;
            _context.SaveChanges();
            return priceAfterBenfits1;
        }
        public IActionResult Estrgaaaa(int orderid, string date, bool isChecked = false)
        {
            // Parse the date string back to a DateTime object
            DateTime selectedDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Get the order from the database
            var order = _context.Orders.Include(o => o.DatesInRange).FirstOrDefault(o => o.Id == orderid);
            var custm = _context.Customers.FirstOrDefault(c => c.Id == order.CustomerId);

            // Find the corresponding date in the DatesInRange list
            var selectedDateEntity = order.DatesInRange.FirstOrDefault(d => d.Date.Date == selectedDate.Date);

            // Check if the date already exists in the DatesInRange list
            if (selectedDateEntity != null)
            {


                selectedDateEntity.IsSelected = isChecked;

                selectedDateEntity.Statuse = " ";
                selectedDateEntity.DateFree = null;
                selectedDateEntity.Paypalce = " ";

                order.DatesInRange.Add(selectedDateEntity);
                // Sort the DatesInRange list based on the Date property and get the selected dates
                //  var selectedDates = order.DatesInRange.Where(d => d.IsSelected).OrderBy(d => d.Date).ToList();

                // Calculate the PriceAfterBenfits based on the selected dates
                //TempData["datedeleted"] = selectedDateEntity.Date.ToString();
                //if (selectedDateEntity != null)
                //{
                //    order.DatesInRange.Remove(selectedDateEntity);
                //}

                order.PriceAfterBenfits = CalculatePriceBenfits(order);

                // Save the changes to the database
                _context.SaveChanges();

                // Return the updated PriceAfterBenfits value
                return RedirectToAction("Index", "Order", new { customerId = order.CustomerId, customername = custm.Name, dateTime = selectedDateEntity.Date });
            }

            return RedirectToAction("Index", "Customer");
        }

        private decimal CalculatePriceBenfits(Order order)
        {
            decimal pricePinft = order.PriceAfterBenfits;
            decimal pricpinfmonth = order.PriceAfterpermonth;



            // Calculate the PriceAfterBenfits by adding the total benefits to the rest price
            decimal priceAfterBenfits1 = pricePinft + pricpinfmonth;
            order.PriceAfterBenfits = priceAfterBenfits1;
            _context.SaveChanges();
            return priceAfterBenfits1;
        }

    }
}
