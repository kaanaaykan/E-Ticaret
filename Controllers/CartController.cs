using E_Ticaret.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticaret.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;

        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }
        public async Task<IActionResult> AddProduct(int productId, int quantity = 1, int redirect = 0)
        {
            var cartNumber = await _cartRepo.AddProduct(productId, quantity);
            if (redirect == 0)
                return Ok(cartNumber);
            return RedirectToAction("UserCart");
        }

        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var cartNumber = await _cartRepo.DeleteProduct(productId);
            return RedirectToAction("UserCart");
        }

        public async Task<IActionResult> UserCart()
        {
            var cart = await _cartRepo.UserCart();
            return View(cart);
        }

        public async Task<IActionResult> ItemsInCart()
        {
            int productInCart = await _cartRepo.GetItemsInCart();
            return View(productInCart);
        }

        public async Task<IActionResult> Purchase()
        {
            bool isPurchased = await _cartRepo.Purchase();
            if (!isPurchased)
                throw new Exception("Sunucu tarafında sıkıntı var");
            return RedirectToAction("Index", "Home");
        }
        }
    }

