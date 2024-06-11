﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models
{
    [Table("ProductImage")]
    public class ProductImage
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProductImgId { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string ProductImgName { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? Url { get; set; }

        //public bool IsDefault { get; set; }
        //
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}