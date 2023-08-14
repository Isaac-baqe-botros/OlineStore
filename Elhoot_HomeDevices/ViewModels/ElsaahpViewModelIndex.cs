using Microsoft.EntityFrameworkCore.Storage;

namespace Elhoot_HomeDevices.ViewModels
{
    public class ElsaahpViewModelIndex
    {
        public int Id { get; set; } 
        public string clientName { get; set; }  
        public string Productname { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal Allpeice { get; set; }   
        public int CountMouth { get; set; }
    }
}
