using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTM.Models
{
    [Table("OrderBook")]
    public class OrderBook
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string CodeOrder { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }
        public decimal TotalMoney { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ReceiveName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ReceiveAddress { get; set; }

        [Column(TypeName = "varchar(64)")]
        public string ReceivePhone { get; set; }

        [Column(TypeName = "ntext")]
        public string Notes { get; set; }
        //khoá ngoại
        public int CustomerID { get; set; }
        public int TransactStatusID { get; set; }
        public int PaymentId { get; set; }
        public Customer Customer { get; set; }
        public Payment Payment { get; set; }
        public TransactStatus TransactStatus { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; }


    }
}
