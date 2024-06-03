using Microsoft.AspNetCore.Mvc.Filters;
using DynamicNavigationMVC.Services.LayoutService;
using Microsoft.AspNetCore.Mvc;

namespace DynamicNavigationMVC.DataFilters
{
    public class LayoutDataFilter : IActionFilter
    {
        private readonly ILayoutDataService _layoutDataService;
        public LayoutDataFilter(ILayoutDataService layoutDataService)
        {
            _layoutDataService = layoutDataService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            if (controller != null)
            {
                var layoutData = _layoutDataService.GetLayoutData();
                controller.ViewData["Categories"] = layoutData;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Not implemented
        }

    }
}
