using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookCart
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Processing;

        public string? ShippingAddress { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; } = 1;
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal UnitPrice{ get; set; }
    }
}