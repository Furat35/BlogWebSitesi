using Microsoft.AspNetCore.Mvc;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            var articles = await _articleService.GetAllByPaggingAsync(categoryId, currentPage, pageSize, isAscending);
            return View(articles);
        }

        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 2, bool isAscending = false)
        {
            var articles = await _articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);
            return View(articles);
        }
    }
}