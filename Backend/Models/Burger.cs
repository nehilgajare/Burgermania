using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Burgermania.Models
{
    [Table("Burger")]
    public class Burger
    { 
        public int BurgerId { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal? Price { get; set; }
        public string? ImageLink { get; set; }
        //public ICollection<Order> Orders { get; set; }
    }

   
}
