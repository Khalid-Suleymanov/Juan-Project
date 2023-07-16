namespace BackendProject.ViewModels
{
    public class BasketViewModel
    {
        public List<BasketItemVM> basketItems { get; set; } = new List<BasketItemVM>();

        public decimal TotalAmount { get; set; }
    }
}
