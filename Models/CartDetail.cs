using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    [Table("CartDetail")]

    public class CartDetail
    {
        public int Id { get; set; }

        [Required]

        public int ShopingCartId { get; set; }

        [Required]

        public int ProductId { get; set; }

        [Required]

        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }

        public Product Product { get; set; }

        public ShopingCart ShopingCart { get; set; }
    }
}
