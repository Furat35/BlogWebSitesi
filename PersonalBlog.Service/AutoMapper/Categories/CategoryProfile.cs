using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Categories;

namespace YoutubeBlog.Service.AutoMapper.Categories
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryAddDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
        }
    }
}
