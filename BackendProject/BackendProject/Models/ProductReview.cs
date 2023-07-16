using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int ProductId { get; set; }
        [Required]
        public byte Rate { get; set; }
        [MaxLength(500)]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public Product Product { get; set; }
        public AppUser AppUser { get; set; }
    }
}
