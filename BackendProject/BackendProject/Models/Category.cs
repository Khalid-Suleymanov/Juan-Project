using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(45)]
        public string Name { get; set; }

        public List<Product> Products { get; set; }
    }
}
