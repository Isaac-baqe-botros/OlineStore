using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
 

namespace Elhoot_HomeDevices.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<StoreDate> storeDates { get; set; }

        public DbSet<SelectedDate> SelectedDates { get; set; }
        public DbSet<Madunaate> madunaates { get; set; }
        public DbSet<Dayeenatey> Dayeenateys { get; set; }
        public DbSet<SelecteddataDyant>  selecteddataDyants { get; set; }
        public DbSet<Elsaahp>Elsaahps { get; set; }

    }
    public class SelectedDate
    {
        public int Id { get; set; }
        public int MadunatID { get; set; }
        public DateTime Date { get; set; }
        public bool IsSelected { get; set; }

        // Navigation property to the associated order
        public Madunaate  Madunaate { get; set; }
        public string Status { get; set; } = "";
        public DateTime? DateFree { get; set; }
        public string? Paypalce { get; set; }
    }

     

    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
         public decimal Totalprice { get; set; }
       
    }
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Count { get; set; }
        public decimal? TotalPriceCount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        

        [Required]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        
        public Category? Category { get; set; }
    }

    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        //[Required]
        [EmailAddress]
        public string? Email { get; set; }

        public string ?Address { get; set; }
        public int Sequance { get; set; }    
        public ICollection<Order>? Orders { get; set; }
        [Phone]
        public string Phone { get; set; }
    }

    public class Order
    {

        


        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public string? productNmae { get; set; }
        public decimal AllPrice { get; set; }
        public decimal PayedPrice { get; set; }
        public decimal RestPrice { get; set; }
        public decimal Penfits { get; set; }
        public decimal PriceAfterBenfits { get; set; }
        public decimal PriceAfterpermonth { get; set; }
        public int Peroid { get; set; }
        [NotMapped]
        public List<DateTime>? SelectedDates { get; set; }=new List<DateTime>();
        public Customer? Customer { get; set; }
        [Required]
        public DateTime Startdate { get; set; } = new DateTime(2023, 1, 1);
        public DateTime Enddate { get; set; }
        public DateTime PayerTime { get; set; }= new DateTime(2023, 1, 1);


        public List<StoreDate>? DatesInRange { get; set; } = new List<StoreDate>();



        public string? Comment { get; set; }
        

      //  public string Status { get; set; }
    }

   public class StoreDate
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public bool IsSelected { get; set; }
        public string? Statuse { get; set; } = ""; 
        public DateTime? DateFree { get; set; }   
        public string? Paypalce { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

       
    }

    public class Madunaate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Money { get; set; }
        public DateTime date { get; set; }
        public decimal Pienfits { get; set; }
        public int? CountMonth { get; set; }
        public List<SelectedDate>? selectedDatesRange { get; set; } = new List<SelectedDate>();
        
    }
    public class Dayeenatey
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Money { get; set; }
        public DateTime date { get; set; }
        public decimal Pienfits { get; set; }
        public int? CountMonth { get; set; }
        public List<SelecteddataDyant>? selectedDatesRange { get; set; } = new List<SelecteddataDyant>();

    }
    public class SelecteddataDyant
    {
        public int Id { get; set; }
        public int DeenID { get; set; }
        public DateTime Date { get; set; }
        public bool IsSelected { get; set; }

        // Navigation property to the associated order
        public Dayeenatey Dayeenatey { get; set; }
        public string Status { get; set; } = "";
        public DateTime? DateFree { get; set; }
        public string? Paypalce { get; set; }
        
    }
    public class Elsaahp
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ProductName { get; set; }
        public decimal Allprice { get; set; }
        public decimal PayPrice { get; set; }
        public DateTime Date    { get; set; } 
        public DateTime DateFree { get; set; }
        public string? Paypalce { get; set; }
        public List<SelectedDateElsaahp>? selectedDatesRange { get; set; } = new List<SelectedDateElsaahp>();
        public int CountMounth { get; set; }    
    }


    public class SelectedDateElsaahp
    {
        public int Id { get; set; }
        public int ElsahpID { get; set; }
        public DateTime Dateofsahp { get; set; }
        public bool IsSelected { get; set; }
        public decimal PayPrice { get; set; }
        // Navigation property to the associated order
        public string Status { get; set; } = "";
        public DateTime? DateFree { get; set; }
        public string? Paypalce { get; set; }
        public Elsaahp Elsaahp { get; set; }
    }
}

 