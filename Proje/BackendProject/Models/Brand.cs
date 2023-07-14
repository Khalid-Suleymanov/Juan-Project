using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
