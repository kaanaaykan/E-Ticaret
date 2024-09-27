using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        internal object Product;

        public int Id { get; set; }

        [Required, MaxLength(20)]

        public string StatusName { get; set; }

        [Required]
        public int OrderStatusId { get; set; }


    }
}
