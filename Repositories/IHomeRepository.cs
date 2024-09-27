using E_Ticaret.Models;

namespace E_Ticaret.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product?>> GetProduct(string sTerm = "", int categoryId = 0);
        Task<IEnumerable<Category>> Category();
    }
}
