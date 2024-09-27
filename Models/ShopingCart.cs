using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Ticaret.Models
{
    [Table("ShopingCart")]
    public class ShopingCart
    {
        public int Id { get; set; }

        [Required]

        public string UserId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<CartDetail> CartDetails { get; set; }
    }
}
