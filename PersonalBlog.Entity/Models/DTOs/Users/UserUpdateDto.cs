using Microsoft.AspNetCore.Http;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Roles;

namespace YoutubeBlog.Entity.Models.DTOs.Users
{
    public class UserUpdateDto 
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid RoleId { get; set; }
        public List<RoleDto> Roles { get; set; }
        public Image Image { get; set; }
        public IFormFile Photo { get; set; }
    }
}
