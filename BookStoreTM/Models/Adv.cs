using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("Adv")]
    public class Adv : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AdvId { get; set; }
        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Images { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Link { get; set; }

        //public int AccountId { get; set; }
        //// khoá ngoại
        //public Account Account { get; set; }
        
    }
}
