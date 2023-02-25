using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Users;

namespace YoutubeBlog.Service.AutoMapper.Users
{
    internal class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, UserAddDto>().ReverseMap();
            CreateMap<AppUser, UserUpdateDto>().ReverseMap();
            CreateMap<AppUser, UserProfileDto>().ReverseMap();
            CreateMap<AppUser, UserRegisterDto>().ReverseMap();
            CreateMap<UserRegisterDto, UserAddDto>();
        }
    }
}
