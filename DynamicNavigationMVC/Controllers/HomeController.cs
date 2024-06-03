using DynamicNavigationMVC.Models;
using DynamicNavigationMVC.Repository;
using DynamicNavigationMVC.Services.ProductService;
using DynamicNavigationMVC.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using System.Diagnostics;
using System.IO;

namespace DynamicNavigationMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService;
        
        public HomeController(IDataAccess<Product> productService, IDataAccess<Category> categoryService)
        {
            _productService = (ProductService)productService;
        }

        public IActionResult Index()
        {   
            IEnumerable<Product> products = _productService.GetAll().Result;
            return View(products);
        }

        public IActionResult ProductByCategory(int id)
        {
            IEnumerable<Product> products = _productService.GetByCategory(id).Result;
            return View("Index", products);
        }
        
    }
}
