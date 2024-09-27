using E_Ticaret.Data;
using E_Ticaret.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Category>> Category()
        {
            return await _db.Category.ToListAsync();
        }
        public async Task<IEnumerable<Product?>> GetProduct(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Product
                                               join category in _db.Category
                                               on product.CategoryId equals category.Id
                                               where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.ProductName.ToLower().StartsWith(sTerm))
                                               select new Product
                                               {
                                                   Id = product.Id,
                                                   Image = product.Image,
                                                   Description = product.Description,
                                                   ProductName = product.ProductName,
                                                   CategoryId = product.CategoryId,
                                                   Price = product.Price,
                                                   CategoryName = category.CategoryName,
                                               }
                           ).ToListAsync();
            if (categoryId > 0)
            {
                products = products.Where(a => a.CategoryId == categoryId).ToList();
            }
            return products;
        }
    }
}
