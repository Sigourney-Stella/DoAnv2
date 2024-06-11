using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("TransactStatus")]
    public class TransactStatus
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TransactStatusID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Status { get; set; }
        public ICollection<OrderBook> OrderBook { get; set; }

    }
}
