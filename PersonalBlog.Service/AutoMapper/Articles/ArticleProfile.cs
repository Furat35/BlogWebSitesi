using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Articles;

namespace YoutubeBlog.Service.AutoMapper.Articles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Article, ArticleAddDto>().ReverseMap();
            CreateMap<Article, ArticleUpdateDto>().ReverseMap();
            CreateMap<ArticleDto, ArticleUpdateDto>().ReverseMap();
        }
    }
}
