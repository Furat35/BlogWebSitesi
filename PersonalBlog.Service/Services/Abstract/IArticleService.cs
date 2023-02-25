using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Articles;

namespace YoutubeBlog.Service.Services.Abstract
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetAllArticlesWithCategoryNonDeletedAsync();
        Task<List<ArticleDto>> GetAllDeletedArticlesWithCategoryAsync();
        Task<string> CreateArticleAsync(ArticleAddDto articleAddDto);
        Task<ArticleDto> GetArticleWithCategoryNonDeletedAsync(Guid articleId);
        Task<string> UpdateArticleAsync(ArticleUpdateDto articleUpdateDto);
        Task<string>  SafeDeleteArticleAsync(Guid articleId);
        Task<string>  UndoDeleteAsync(Guid articleId);
        Task<ArticleListDto> GetAllByPaggingAsync(Guid? categoryId, int currentPage = 1, int pageSize = 3, bool isAscending = false);
        Task<ArticleListDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false);
        Task<List<ArticleDto>> GetLastNPostsAsync(int count);
    }
}
