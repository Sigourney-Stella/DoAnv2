using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("Payment")]
    public class Payment : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }

        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar(100)")]
        public string PaymentName { get; set; }
        public ICollection<OrderBook> OrderBooks { get; set; }

        //khoas ngoại

    }
}
