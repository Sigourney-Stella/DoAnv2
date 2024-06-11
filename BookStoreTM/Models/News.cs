using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("News")]
    public class News : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NewsId { get; set; }

        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string Title { get; set; }
        public string Alias { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? Image { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Detail { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoTitle { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoKeywords { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? SeoDescription { get; set; }

        public bool IsActicve { get; set; }
        //
        //public int CategoryId { get; set; }
        ////
        //public Category Category { get; set; }
    }

}
