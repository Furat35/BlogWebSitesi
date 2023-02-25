using Microsoft.AspNetCore.Mvc;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public async Task<IActionResult> Index(Guid articleId)
        {
            var article = await _articleService.GetArticleWithCategoryNonDeletedAsync(articleId);
            return View(article);
        }
    }
}
