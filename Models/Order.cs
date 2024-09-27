using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }

        [Required]

        public string UserId { get; set; }

        [Required]

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int OrderStatusId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public OrderStatus OrderStatus { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}
