using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("ProductCategory")]
    public class ProductCategory : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProductCategoryId { get; set; }

        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string Name { get; set; }
        public string Alias { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        public string? Icon { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoTitle { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoKeywords { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoDescription { get; set; }
        public ICollection<Product> Products { get;}
    }
}
