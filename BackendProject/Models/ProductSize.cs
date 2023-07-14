namespace BackendProject.Models
{
    public class ProductSize
    {
        public int SizeId { get; set; }

        public int ProductId { get; set; }

        public Size Size { get; set; }

        public Product Product { get; set; }

    }
}
