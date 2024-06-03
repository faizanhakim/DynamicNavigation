using DynamicNavigationMVC.Models;
using DynamicNavigationMVC.Repository;
using DynamicNavigationMVC.Services.CategoryService;
using System.Collections;

namespace DynamicNavigationMVC.Services.LayoutService
{
    public class LayoutDataService : ILayoutDataService
    {   
        private readonly CategoryService.CategoryService _categoryService;
        
        public LayoutDataService(IDataAccess<Category> categoryService)
        {
            _categoryService = (CategoryService.CategoryService)categoryService;
        }

        public IEnumerable<Category> GetLayoutData()
        {
            IEnumerable<Category>? data = _categoryService.GetAllTopLevelCategories().Result;
            if (data == null)
            {
                data = new List<Category>();
            }
            return data;
        }
    }
}
