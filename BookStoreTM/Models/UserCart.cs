using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("UserCart")]
    public class UserCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public int SoLuong { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale { get; set; }
        public decimal TotalPrice { get; set; }
        public string ProductName { get; set; }
        public string ProductImg { get; set; }

        public int CustomerID { get; set; }
        public int ProductId { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
