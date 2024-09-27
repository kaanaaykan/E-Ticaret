using E_Ticaret.Data;
using E_Ticaret.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Ticaret.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> AddProduct(int productId, int quantity)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {

                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Kullanıcı oturum açmadı");

                var cart = await GetCart(userId);

                if (cart is null)
                {
                    cart = new ShopingCart
                    {
                        UserId = userId
                    };
                    _db.Cart.Add(cart);
                }

                _db.SaveChanges();

                var cartProduct = _db.CartDetail.FirstOrDefault(a => a.ShopingCartId == cart.Id && a.ProductId == productId);

                if (cartProduct is not null)
                {
                    cartProduct.Quantity += quantity;
                }
                else
                {
                    var product = _db.Product.Find(productId);
                    cartProduct = new CartDetail
                    {
                        ProductId = productId,
                        ShopingCartId = cart.Id,
                        Quantity = quantity,
                        UnitPrice = product.Price
                    };
                    _db.CartDetail.Add(cartProduct);
                }

                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var itemsInCart = await GetItemsInCart(userId);
            return itemsInCart;
        }

        public async Task<int> DeleteProduct(int productId)
        {
            string userId = GetUserId();
            try
            {

                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Kullanıcı oturum açmadı");

                var cart = await GetCart(userId);

                if (cart is null)
                    throw new Exception("Geçersiz sepet");

                var cartProduct = _db.CartDetail.FirstOrDefault(a => a.ShopingCartId == cart.Id && a.ProductId == productId);

                if (cartProduct is null)
                    throw new Exception("Sepet Boş");

                else if (cartProduct.Quantity == 1)
                    _db.CartDetail.Remove(cartProduct);
                else
                    cartProduct.Quantity = cartProduct.Quantity - 1;

                _db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            var itemsInCart = await GetItemsInCart(userId);
            return itemsInCart;
        }

        public async Task<ShopingCart> UserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("geçersiz kullanıcı kimliği");
            var shoppingCart = await _db.Cart
                                    .Include(a => a.CartDetails)
                                    .ThenInclude(a => a.Product)
                                    .ThenInclude(a => a.Category)
                                    .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }

        public async Task<ShopingCart> GetCart(string userId)
        {
            var cart = await _db.Cart.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetItemsInCart(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.Cart
                              join cartDetails in _db.CartDetail
                              on cart.Id equals cartDetails.ShopingCartId
                              select new { cartDetails.Id }
                              ).ToListAsync();
            return data.Count;
        }

        public async Task<bool> Purchase()
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("Kullanıcı oturum açmadı");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Geçersiz sepet");
                var cartDetails = _db.CartDetail
                                        .Where(a => a.ShopingCartId == cart.Id).ToList();
                if (cartDetails.Count == 0)
                    throw new Exception("Sepet boş");
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    OrderStatusId = 1
                };
                _db.Order.Add(order);
                _db.SaveChanges();
                foreach (var item in cartDetails)
                {
                    var orderDetails = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetail.Add(orderDetails);
                }
                _db.SaveChanges();

                _db.CartDetail.RemoveRange(cartDetails);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}

