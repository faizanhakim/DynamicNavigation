using DynamicNavigationMVC.Models;


namespace DynamicNavigationMVC.Services.LayoutService
{
    public interface ILayoutDataService
    {
        IEnumerable<Category> GetLayoutData();
    }
}
