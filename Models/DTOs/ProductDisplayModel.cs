namespace E_Ticaret.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Product { get; set; }

        public IEnumerable<Category> Category { get; set; }

        public string STerm { get; set; } = "";

        public int CategoryId { get; set; } = 0;
    }
}
