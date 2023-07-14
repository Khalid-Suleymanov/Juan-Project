using Pustok.Attributes.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models
{
    public class FourBrand
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Image { get; set; }
        [NotMapped]
        [MaxFileLength(2097152)]
        [AllowedContentTypes("image/jpeg", "image/png", "image/jpg")]
        public IFormFile ImageFile { get; set; }
    }
}
