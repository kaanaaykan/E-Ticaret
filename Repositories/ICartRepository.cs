using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddProduct(int productId, int quantity);
        Task<int> DeleteProduct(int productId);
        Task<ShopingCart> UserCart();
        Task<int> GetItemsInCart(string userId = "");
        Task<ShopingCart> GetCart(string userId);
        Task<bool> Purchase();
    }
}
