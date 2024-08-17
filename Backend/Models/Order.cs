using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Burgermania.Models
{
    [Table("Order")]
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int OrderId { get; set; }

        [JsonPropertyName("burgerId")]
        public int BurgerId { get; set; }

        [JsonPropertyName("itemName")]
        public string? ItemName {  get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("orderDate")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        //public ICollection<Order>? Items { get; set; } = new List<Order>();
        //[ForeignKey("UserId")]
        // public User User { get; set; }

        //[ForeignKey("BurgerId")]
        //public Burger Burger { get; set; }
    }
}
