using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DynamicNavigationMVC.Models;
using DynamicNavigationMVC.data;
using DynamicNavigationMVC.Services.CategoryService;
using DynamicNavigationMVC.Repository;

namespace DynamicNavigationMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(IDataAccess<Category> categoryService)
        {
            _categoryService = (CategoryService)categoryService;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAll());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int id)
        {
            
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<Category> categories = (List<Category>) await _categoryService.GetAll();

            SelectList selectedCategories;

            selectedCategories = new SelectList(categories, "Id", "Name");
            
            ViewData["categories"] = selectedCategories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SelectedParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                var selectedCategory = await _categoryService.GetById(category.SelectedParentCategoryId);
                category.ParentCategory.Add(selectedCategory);
                await _categoryService.Add(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
   
        public async Task<IActionResult> Edit(int id)
        {

            List<Category> categories = (List<Category>)await _categoryService.GetAll();
            
            var category = categories.FirstOrDefault(c=>c.Id == id);
            categories.Remove(category!);

            SelectList selectedCategories;

            if (category!.ParentCategory.Count == 0) 
            {
                selectedCategories = new SelectList(categories, "Id", "Name");
            }
            else
            {
                selectedCategories = new SelectList(categories, "Id", "Name", category!.ParentCategory[0].Id);
            }
        
            ViewData["categories"] = selectedCategories;

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SelectedParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                var selectedCategory = await _categoryService.GetById(category.SelectedParentCategoryId);
                category.ParentCategory.Add(selectedCategory);
                

                await _categoryService.Update(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.Delete(id);
           return RedirectToAction(nameof(Index));
        }
    }
}
