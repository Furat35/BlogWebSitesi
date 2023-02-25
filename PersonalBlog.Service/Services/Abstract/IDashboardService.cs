using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;

namespace YoutubeBlog.Service.Services.Abstract
{
    public interface IDashboardService
    {
        Task<List<int>> GetYearlyArticleCountsAsync();
        Task<int> GetTotalArticleCountAsync(Expression<Func<Article, bool>> predicate = null);
        Task<int> GetTotalCategoryCountAsync(Expression<Func<Category, bool>> predicate = null);
        Task<int> GetAllUsersAsync(Expression<Func<AppUser, bool>> predicate = null);
    }
}
