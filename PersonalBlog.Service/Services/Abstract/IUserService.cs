using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Users;

namespace YoutubeBlog.Service.Services.Abstract
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersWithRoleAsync(Expression<Func<AppUser, bool>> predicate = null);
        Task<List<AppRole>> GetAllRolesAsync();
        Task<IdentityResult> CreateUserAsync(UserAddDto userAddDto);
        Task<AppUser> GetAppUserByIdAsync(Guid userId);
        Task<(IdentityResult identityResult, string userName)> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<(IdentityResult identityResult, string? email)> SafeDeleteUserAsync(Guid userId);
        Task<string> GetUserRoleAsync(AppUser user);
        Task<UserProfileDto> GetUserProfileAsync();
        Task<bool> UserProfileUpdateAsync(UserProfileDto userProfileDto);
        Task<AppUser> GetAppUserByIdIncludeImageAsync(Guid userId);
        Task<AppUser> GetAppUserByEmailAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync(Expression<Func<AppUser, bool>> predicate = null);
        Task<string> UndoDeleteAsync(Guid userId);

    }
}
