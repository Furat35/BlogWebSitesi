using Microsoft.AspNetCore.Mvc;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Web.ViewComponents.Category
{
    public class GetCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public GetCategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetFirstNCategoriesAsync(7);
            return View(categories);
        }
    }
}
