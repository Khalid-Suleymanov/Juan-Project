using BackendProject.Enums;
using System.ComponentModel.DataAnnotations;

namespace BackendProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string FullName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(150)]
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        [MaxLength(300)]
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AppUserId { get; set; }
        public OrderStatus Status { get; set; }
        public AppUser AppUser { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
