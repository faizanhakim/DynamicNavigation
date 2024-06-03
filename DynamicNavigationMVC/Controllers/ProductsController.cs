using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DynamicNavigationMVC.Models;
using DynamicNavigationMVC.data;
using DynamicNavigationMVC.Services.ProductService;
using DynamicNavigationMVC.Repository;
using DynamicNavigationMVC.Services.CategoryService;

namespace DynamicNavigationMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ProductsController(IDataAccess<Product> productService, IDataAccess<Category> categoryService)
        {
            _productService = (ProductService)productService;
            _categoryService = (CategoryService)categoryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetAll());
        }

        public async Task<IActionResult> Details(int id)
        {
            
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Category> categories = (List<Category>)await _categoryService.GetLeafCategories();

            SelectList selectedCategories;

            selectedCategories = new SelectList(categories, "Id", "Name");

            ViewData["categories"] = selectedCategories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,SelectedParentCategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                var selectedCategory = await _categoryService.GetById(product.SelectedParentCategoryId);
                product.Category.Add(selectedCategory);
                await _productService.Add(product);
                return (RedirectToAction(nameof(Index)));
            }
            return (View(product));
        }
  
        public async Task<IActionResult> Edit(int id)
        {
            var product = _productService.GetById(id).Result;

            List<Category> categories = (List<Category>)await _categoryService.GetLeafCategories();
            
            SelectList selectedCategories;

            if (product!.Category.Count == 0) 
            {
                selectedCategories = new SelectList(categories, "Id", "Name");
            }
            else
            {
                selectedCategories = new SelectList(categories, "Id", "Name", product.Category[0].Id);
            }
        
            ViewData["categories"] = selectedCategories;

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,SelectedParentCategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var selectedCategory = await _categoryService.GetById(product.SelectedParentCategoryId);
                product.Category.Clear();
                product.Category.Add(selectedCategory);

                await _productService.Update(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
 
        public async Task<IActionResult> Delete(int id)
        {
            var product = _productService.GetById(id).Result;

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.Delete(id);            
            return RedirectToAction(nameof(Index));
        }

        
    }
}
