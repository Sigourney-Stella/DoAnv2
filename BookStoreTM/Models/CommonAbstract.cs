using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    public abstract class CommonAbstract
    {
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? CreatedBy { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? UpdatedBy { get; set; }
    }
}
