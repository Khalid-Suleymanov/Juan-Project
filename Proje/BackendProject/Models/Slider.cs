using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pustok.Attributes.CustomValidationAttributes;

namespace BackendProject.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public int Order { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title1 { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title2 { get; set; }
        [Required]
        [MaxLength(150)]
        public string Desc { get; set; }
        [Required]
        [MaxLength(150)]
        public string BtnText { get; set; }
        [Required]
        [MaxLength(150)]
        public string BtnUrl { get; set; }
        [MaxLength(100)]
        public string Image { get; set; }
        [NotMapped]
        [MaxFileLength(2097152)]
        [AllowedContentTypes("image/jpeg", "image/png", "image/jpg")]
        public IFormFile ImageFile { get; set; }
    }
}
