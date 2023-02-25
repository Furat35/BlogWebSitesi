using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Data.Context;
using YoutubeBlog.Data.Repositories.Abstract;
using YoutubeBlog.Data.Repositories.Concrete;
using YoutubeBlog.Data.UnitOfWorks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Articles;

namespace YoutubeBlog.Data.Extensions
{
    public static class DataLayerExtension
    {
        public static IServiceCollection LoadDataLayerExtension(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
