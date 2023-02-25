using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Roles;

namespace YoutubeBlog.Service.Services.Abstract
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync(Expression<Func<AppRole, bool>> predicate = null);
        Task<string> CreateRoleAsync(RoleAddDto roleDto);
        Task<string> DeleteRoleAsync(Guid roleId);
        Task<string> UpdateRoleAsync(RoleUpdateDto roleUpdateDto);
        Task<AppRole> GetRoleByGuidAsync(Guid roleId);
        Task<string> GetRoleGuidAsync(string name);
        Task<string> SafeDeleteRoleAsync(Guid roleId);
    }
}
