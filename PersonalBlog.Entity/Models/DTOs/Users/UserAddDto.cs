using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Roles;

namespace YoutubeBlog.Entity.Models.DTOs.Users
{
    public class UserAddDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
        public List<RoleDto> Roles { get; set; }
        public IFormFile Photo { get; set; }
    }
}
