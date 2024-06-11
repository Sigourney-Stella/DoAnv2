using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderDetailsId { get; set; }

        [Range(0, double.MaxValue)]
        public Decimal Price { get; set; }
        public string Code { get; set; }

        [Column(TypeName = "int")]
        public int Quatity { get; set; }

        [Range(0, double.MaxValue)]
        public Decimal TotalMoney { get; set; }

        //khoá ngoại
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        //
        public OrderBook OrderBook { get; set; }
        public Product Product { get; set; }    
    }
}
