using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Data.UnitOfWorks;
using YoutubeBlog.Service.FluentValidations;
using YoutubeBlog.Service.Helpers.Images;
using YoutubeBlog.Service.Services.Abstract;
using YoutubeBlog.Service.Services.Concrete;

namespace YoutubeBlog.Service.Extensions
{
    public static class ServiceLayerExtensions
    {
        public static IServiceCollection LoadServiceLayerExtension(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews()
                .AddFluentValidation(opt =>
            {
                opt.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
                opt.DisableDataAnnotationsValidation = true;
                opt.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
            });

            

            services.AddAutoMapper(assembly);
            return services;
        }
    }
}
