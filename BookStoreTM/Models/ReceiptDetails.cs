using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("ReceiptDetails")]
    public class ReceiptDetails
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ReceiptDetailsId { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string NameProduct { get; set; }

        [Range(0, double.MaxValue)]
        [Required(ErrorMessage ="Giá tiền không được để trống")]
        public Decimal Price { get; set; }

        [Column(TypeName = "int")]
        [Required(ErrorMessage = "Số lượng không được để trống")]
        public int Quatity { get; set; }

        //khoá ngoại
        public int ReceiptId { get; set; }
        //
        public Receipt Receipt { get; set; }
    }
}
