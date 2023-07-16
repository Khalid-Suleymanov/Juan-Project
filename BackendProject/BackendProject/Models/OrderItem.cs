namespace BackendProject.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public decimal UnitSalePrice { get; set; }
        public decimal UnitCostPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
