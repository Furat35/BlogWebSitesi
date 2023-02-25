using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YoutubeBlog.Data.UnitOfWorks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Enums;
using YoutubeBlog.Entity.Models.DTOs.Articles;
using YoutubeBlog.Service.Extensions;
using YoutubeBlog.Service.Helpers.Images;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Service.Services.Concrete
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        private readonly IImageHelper _imageHelper;
        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
            _imageHelper = imageHelper;
        }

        public async Task<List<ArticleDto>> GetLastNPostsAsync(int count)
        {
            var articles = (await _unitOfWork.GetRepository<Article>().GetAllAsync()).OrderByDescending(_ => _.CreatedDate).Take(count);
            return _mapper.Map<List<ArticleDto>>(articles);
        }

        public async Task<ArticleListDto> GetAllByPaggingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;

            var articles = categoryId == null
                ? await _unitOfWork.GetRepository<Article>().GetAllAsync(_ => !_.isDeleted, c => c.Category, i => i.Image, u => u.User)
                : await _unitOfWork.GetRepository<Article>().GetAllAsync(_ => _.CategoryId == categoryId && !_.isDeleted, c => c.Category, i => i.Image, u => u.User);

            var sortedArticles = isAscending
                ? articles.OrderBy(_ => _.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(_ => _.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new ArticleListDto
            {
                Articles = sortedArticles,
                CategoryId = categoryId == null ? null : categoryId,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };
        }

        public async Task<string> CreateArticleAsync(ArticleAddDto articleAddDto)
        {
            var userId = _user.GetLoggedInUserId();
            var userEmail = _user.GetLoggedInEmail();

            var imageUpload = await _imageHelper.Upload(articleAddDto.Title, articleAddDto.Photo, ImageType.Post);
            Image image = new(imageUpload.FullName, articleAddDto.Photo.ContentType, userEmail);
            await _unitOfWork.GetRepository<Image>().AddAsync(image);

            var article = new Article(articleAddDto.Title, articleAddDto.Content, userId, userEmail, articleAddDto.CategoryId, image.Id);

            await _unitOfWork.GetRepository<Article>().AddAsync(article);
            int effectedRows = await _unitOfWork.SaveAsync();

            return effectedRows > 0 ? articleAddDto.Title : null;
        }

        //public async Task<List<ArticleDto>> GetAllArticlesAsync()
        //{
        //    var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync();
        //    return _mapper.Map<List<ArticleDto>>(articles);
        //}

        public async Task<List<ArticleDto>> GetAllArticlesWithCategoryNonDeletedAsync()
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(predicate: _ => !_.isDeleted, includeProperties: include => include.Category);
            return _mapper.Map<List<ArticleDto>>(articles);
        }

        private async Task<Article?> ArticleExistAsync(Guid articleId, bool isDeleted)
        {
            try
            {
                Article article = await _unitOfWork.GetRepository<Article>().GetAsync(predicate: _ => _.isDeleted == isDeleted && _.Id == articleId, include => include.Category, include => include.Image);
                return article;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ArticleDto> GetArticleWithCategoryNonDeletedAsync(Guid articleId)
        {
            Article article = await ArticleExistAsync(articleId, false);
            return article != null ? _mapper.Map<ArticleDto>(article) : null;
        }

        public async Task<string> UpdateArticleAsync(ArticleUpdateDto articleUpdateDto)
        {
            Article article = await ArticleExistAsync(articleUpdateDto.Id, false);

            if (article != null)
            {
                if (articleUpdateDto.Photo != null)
                {
                    if (article.Image is not null)
                    {
                        article.Image.isDeleted = true;
                        _imageHelper.Delete(article.Image.FileName);
                    }

                    var imageUploade = await _imageHelper.Upload(articleUpdateDto.Title, articleUpdateDto.Photo, ImageType.Post);
                    Image image = new(imageUploade.FullName, articleUpdateDto.Photo.ContentType, _user.GetLoggedInEmail());
                    await _unitOfWork.GetRepository<Image>().AddAsync(image);

                    article.ImageId = image.Id;
                }

                string articleTitle = article.Title;

                article.Title = articleUpdateDto.Title;
                article.CategoryId = articleUpdateDto.CategoryId;
                article.Content = articleUpdateDto.Content;

                article.ModifiedDate = DateTime.Now;
                article.ModifiedBy = _user.GetLoggedInEmail();

                await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
                int effectedRows = await _unitOfWork.SaveAsync();

                return effectedRows > 0 ? articleTitle : null;
            }

            return null;
        }

        public async Task<string> SafeDeleteArticleAsync(Guid articleId)
        {
            Article article = await ArticleExistAsync(articleId, false);

            if (article != null)
            {
                if (!article.isDeleted)
                {
                    article.isDeleted = true;
                    article.DeletedDate = DateTime.Now;
                    article.DeletedBy = _user.GetLoggedInEmail();

                    await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
                    int effectedRows = await _unitOfWork.SaveAsync();

                    return effectedRows > 0 ? article.Title : null;
                }
            }

            return null;
        }

        public async Task<List<ArticleDto>> GetAllDeletedArticlesWithCategoryAsync()
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(predicate: _ => _.isDeleted, include => include.Category, include => include.Image);
            return _mapper.Map<List<ArticleDto>>(articles);
        }

        public async Task<string> UndoDeleteAsync(Guid articleId)
        {
            Article? article = await ArticleExistAsync(articleId, true);

            if (article != null)
            {
                article.isDeleted = false;
                article.DeletedDate = null;
                article.DeletedBy = null;

                await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
                int effectedRows = await _unitOfWork.SaveAsync();

                return effectedRows > 0 ? article.Title : null;
            }

            return null;
        }

        public async Task<ArticleListDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;

            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(_ => !_.isDeleted &&
            (_.Title.Contains(keyword) || _.Content.Contains(keyword) || _.Category.Name.Contains(keyword)), c => c.Category, i => i.Image, u => u.User);

            var sortedArticles = isAscending
                ? articles.OrderBy(_ => _.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(_ => _.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new ArticleListDto
            {
                Articles = sortedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            };
        }
    }
}
