using BackendProject.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pustok.Attributes.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(80)]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountedPrice { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Required]
        public bool StockStatus { get; set; }
        public bool IsDeleted { get; set; }
        public List<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
        public List<Image> Images { get; set; } = new List<Image>();
        [NotMapped]
        public List<int> SizeIds { get; set; } = new List<int>();
        [NotMapped]
        [MaxFileLength(2097152)]
        [AllowedContentTypes("image/png", "image/jpeg", "image/jpg")]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        [MaxFileLength(2097152)]
        [AllowedContentTypes("image/png", "image/jpeg", "image/jpg")]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public GenderStatus Status { get; set; }
        public bool IsNew { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }

    }
}
