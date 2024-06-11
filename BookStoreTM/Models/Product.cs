using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("Product")]
    public class Product : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string ProductName { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Author { get; set; }
        
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Code { get; set; }
        public string Alias { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Detail { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public int? Notes { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Images { get; set; }

        [Range(0, double.MaxValue)]
        public Decimal OriginalPrice { get; set; }

        [Range(0,double.MaxValue)]
        public Decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public Decimal? PriceSale { get; set; }

        [Column(TypeName = "int")]
        public int Quatity { get; set; }
        public bool IsActicve { get; set; }
        public bool IsHome { get; set; }
        public bool IsSale { get; set; }
        //public bool IsHot { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoTitle { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoKeywords { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoDescription { get; set; }

        // khoá ngoại
        public int ProductCategoryId { get; set; }
        public int PublisherId { get; set; }
        //
        public ProductCategory ProductCategory { get; set; }
        public Publisher Publisher { get; set; }
        //
        public ICollection<OrderDetails> OrderDetails { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<UserCart> UserCarts { get; set; }

    }
}
