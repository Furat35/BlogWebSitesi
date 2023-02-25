using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Roles;

namespace YoutubeBlog.Service.AutoMapper.Roles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<AppRole, RoleDto>().ReverseMap();
            CreateMap<AppRole, RoleAddDto>().ReverseMap();
            CreateMap<AppRole, RoleUpdateDto>().ReverseMap();
        }
    }
}
