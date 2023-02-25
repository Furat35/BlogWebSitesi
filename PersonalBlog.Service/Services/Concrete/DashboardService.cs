using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Data.UnitOfWorks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Service.Services.Concrete
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<int>> GetYearlyArticleCountsAsync()
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(_ => !_.isDeleted);

            var startDate = DateTime.Now.Date;
            startDate = new DateTime(startDate.Year, 1, 1);

            List<int> datas = new();
            for (int i = 1; i <= 12; i++)
            {
                var startedDate = new DateTime(startDate.Year, i, 1);
                var endedDate = startedDate.AddMonths(1);
                var data = articles.Where(_ => _.CreatedDate >= startedDate && _.CreatedDate < endedDate).Count();
                datas.Add(data);
            }

            return datas;
        }

        public async Task<int> GetTotalArticleCountAsync(Expression<Func<Article, bool>> predicate = null)
        {
            int articleCount = await _unitOfWork.GetRepository<Article>().CountAsync(predicate);
            return articleCount;
        }

        public async Task<int> GetTotalCategoryCountAsync(Expression<Func<Category, bool>> predicate = null)
        {
            int categoryCount = await _unitOfWork.GetRepository<Category>().CountAsync(predicate);
            return categoryCount;
        }

        public async Task<int> GetAllUsersAsync(Expression<Func<AppUser, bool>> predicate = null)
        {
            var users = await _unitOfWork.GetRepository<AppUser>().GetAllAsync(predicate);
            return users.Count;
        }


    }
}
