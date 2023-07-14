using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models
{
    public class Image
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(120)]
        public string ImageName { get; set; }
        [Required]
        public bool ImageStatus { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
