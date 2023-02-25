using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Articles;
using YoutubeBlog.Service.Services.Abstract;
using YoutubeBlog.Web.Const;
using YoutubeBlog.Web.ResultMessages;

namespace YoutubeBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<Article> _articleValidator;
        private readonly IToastNotification _toast;
        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper, IValidator<Article> articleValidator,
            IToastNotification toast)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _mapper = mapper;
            _articleValidator = articleValidator;
            _toast = toast;
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin},{RoleConst.User}")]
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticlesWithCategoryNonDeletedAsync();
            return View(articles);
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> DeletedArticle()
        {
            var articles = await _articleService.GetAllDeletedArticlesWithCategoryAsync();
            return View(articles);
        }


        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryService.GetAllCategoriesNonDeleteAsync();
            return View(new ArticleAddDto
            {
                Categories = categories
            });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Add(ArticleAddDto articleAddDto)
        {
            var map = _mapper.Map<Article>(articleAddDto);
            var result = await _articleValidator.ValidateAsync(map);

            if (result.IsValid)
            {
                var title = await _articleService.CreateArticleAsync(articleAddDto);

                if(title != null)
                {
                    _toast.AddSuccessToastMessage(Messages.Article.Add(title), new ToastrOptions { Title = Messages.ToastTitle.Success });
                }
                else
                {
                    _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.ToastTitle.Error });
                }

                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
                articleAddDto.Categories = await _categoryService.GetAllCategoriesNonDeleteAsync();

                return View(articleAddDto);
            }
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var article = await _articleService.GetArticleWithCategoryNonDeletedAsync(articleId);

            if (article != null)
            {
                var articleUpdateDto = _mapper.Map<ArticleUpdateDto>(article);
                articleUpdateDto.Categories = await _categoryService.GetAllCategoriesNonDeleteAsync();

                return View(articleUpdateDto);
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Update(ArticleUpdateDto articleUpdateDto)
        {
            var map = _mapper.Map<Article>(articleUpdateDto);
            var result = await _articleValidator.ValidateAsync(map);

            if (result.IsValid)
            {
                var title = await _articleService.UpdateArticleAsync(articleUpdateDto);

                if (title != null)
                {
                    _toast.AddSuccessToastMessage(Messages.Article.Update(title), new ToastrOptions { Title = Messages.ToastTitle.Success });
                }
                else
                {
                    _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.ToastTitle.Error });
                }

                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);
                articleUpdateDto.Categories = await _categoryService.GetAllCategoriesNonDeleteAsync();

                return View(articleUpdateDto);
            }
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Delete(Guid articleId)
        {
            var title = await _articleService.SafeDeleteArticleAsync(articleId);

            if (title != null)
            {
                _toast.AddSuccessToastMessage(Messages.Article.Delete(title), new ToastrOptions { Title = Messages.ToastTitle.Success });
            }
            else
            {
                _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.ToastTitle.Error });
            }

            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> UndoDelete(Guid articleId)
        {
            var title = await _articleService.UndoDeleteAsync(articleId);

            if (title != null)
            {
                _toast.AddSuccessToastMessage(Messages.Article.UndoDelete(title), new ToastrOptions { Title = Messages.ToastTitle.Success });
            }
            else
            {
                _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.ToastTitle.Error });
            }

            return RedirectToAction("DeletedArticle", "Article", new { Area = "Admin" });
        }
    }
}
