using Pustok.Attributes.CustomValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendProject.Models
{
    public class SportOff
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        public string Title { get; set; }
        [MaxLength(50)]
        public string Desc1 { get; set; }
        [MaxLength(50)]
        public string Desc2 { get; set; }
        [MaxLength(150)]
        public string Image { get; set; }
        [NotMapped]
        [MaxFileLength(2097152)]
        [AllowedContentTypes("image/jpeg", "image/png", "image/jpg")]
        public IFormFile ImageFile { get; set; }

    }
}
