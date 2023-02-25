using Microsoft.AspNetCore.Mvc;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Web.ViewComponents.Article
{
    public class GetArticlesViewComponent : ViewComponent
    {
        private readonly IArticleService _articleService;

        public GetArticlesViewComponent(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var articles = await _articleService.GetLastNPostsAsync(5);
            return View(articles);
        }
    }
}
