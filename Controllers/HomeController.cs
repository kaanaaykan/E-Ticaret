using E_Ticaret.Models;
using E_Ticaret.Models.DTOs;
using E_Ticaret.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Ticaret.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string sterm = "", int categoryId = 0)
        {
            IEnumerable<Product> products = await _homeRepository.GetProduct(sterm, categoryId);
            IEnumerable<Category> categories = await _homeRepository.Category();
            ProductDisplayModel productModel = new ProductDisplayModel()
            {
                Product = products,
                Category = categories,
                STerm = sterm,
                CategoryId = categoryId
            };
            return View(productModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
