using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("Publisher")]
    public class Publisher
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PublisherId { get; set; }

        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string PublisherName { get; set; }
        public string Alias { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Email { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Address { get; set; }

        [Column(TypeName = "varchar(64)")]
        public string? Phone { get; set; }
        public ICollection<Product> Products { get; }
    }
}
