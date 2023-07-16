using BackendProject.Models;

namespace BackendProject.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
        public ProductReview Review { get; set; }
    }
}
