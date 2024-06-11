using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("Receipt")]
    public class Receipt : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ReceiptId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string? Code { get; set; }
        public decimal TotalMoney { get; set; }

        //[Column(TypeName = "datetime")]
        //public DateTime? ReceiptDate { get; set; }
        [Column(TypeName = "ntext")]
        public string Notes { get; set; }
        public ICollection<ReceiptDetails> ReceiptDetails { get; set; }

    }
}
