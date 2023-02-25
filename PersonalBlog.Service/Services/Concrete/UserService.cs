using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Data.UnitOfWorks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Enums;
using YoutubeBlog.Entity.Models.DTOs.Articles;
using YoutubeBlog.Entity.Models.DTOs.Users;
using YoutubeBlog.Service.Extensions;
using YoutubeBlog.Service.Helpers.Images;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Service.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageHelper _imageHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserService(IUnitOfWork unitOfWork, IImageHelper imageHelper, IHttpContextAccessor httpContextAccessor, IMapper mapper, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _imageHelper = imageHelper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateUserAsync(UserAddDto userAddDto)
        {

            var map = _mapper.Map<AppUser>(userAddDto);

            map.UserName = userAddDto.Email;
            map.IsDeleted = false;

            var imageUpload = await _imageHelper.Upload(userAddDto.Email, userAddDto.Photo, ImageType.User);
            Image image = new(imageUpload.FullName, userAddDto.Photo.ContentType, LogedInUserExtensions.GetLoggedInEmail(_user));
            await _unitOfWork.GetRepository<Image>().AddAsync(image);
            await _unitOfWork.SaveAsync();

            map.ImageId = image.Id;

            var createResult = await _userManager.CreateAsync(map, String.IsNullOrEmpty(userAddDto.Password) ? "" : userAddDto.Password);

            if (createResult.Succeeded)
            {
                var roleExist = await _roleManager.FindByIdAsync(userAddDto.RoleId.ToString());
                string findRole = roleExist != null ? roleExist.Name : "User";

                if(findRole.ToUpper() == "SUPERADMIN")
                {
                    findRole = "User";
                }

                await _userManager.AddToRoleAsync(map, findRole);
            }

            return createResult;
        }

        public async Task<(IdentityResult identityResult, string? email)> SafeDeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var validRole = await _userManager.IsInRoleAsync(user, "SUPERADMIN");

            if (user != null && !validRole)
            {
                user.IsDeleted = true;
                var result = await _userManager.UpdateAsync(user);

                return result.Succeeded ?  (result, user.Email) :  (result, null);
            }

            return (null, null);
        }

        public async Task<List<AppRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<List<UserDto>> GetAllUsersAsync(Expression<Func<AppUser, bool>> predicate = null)
        {
            return _mapper.Map<List<UserDto>>(await _userManager.Users.Where(predicate).ToListAsync());
        }

        public async Task<List<UserDto>> GetAllUsersWithRoleAsync(Expression<Func<AppUser, bool>> predicate = null)
        {
            var users = predicate != null ? await _userManager.Users.Where(predicate).ToListAsync() : await _userManager.Users.ToListAsync();
            var map = _mapper.Map<List<UserDto>>(users);

            foreach (var user in map)
            {
                var findUser = await _userManager.FindByIdAsync(user.Id.ToString());
                var role = await _userManager.GetRolesAsync(findUser);

                if(role.First().ToUpper() != "SUPERADMIN")
                {
                    user.Role = role.First();
                }
                else
                {
                    user.Role = null;
                }
            }

            return map.Where(_ => _.Role != null).ToList();
        }

        public async Task<AppUser> GetAppUserByIdAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<AppUser> GetAppUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<AppUser> GetAppUserByIdIncludeImageAsync(Guid userId)
        {
            AppUser user;

            try
            {
                user = await _unitOfWork.GetRepository<AppUser>().GetAsync(_ => _.Id == userId, i => i.Image);
            }
            catch 
            {
                return null;
            }

            return user;
        }

        public async Task<string> GetUserRoleAsync(AppUser user)
        {
            return (await _userManager.GetRolesAsync(user)).First();
        }

        public async Task<(IdentityResult identityResult, string userName)> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            AppUser user = await _unitOfWork.GetRepository<AppUser>().GetAsync(_ => _.Id == userUpdateDto.Id, i => i.Image);
           
            var role = await GetUserRoleAsync(user);

            if (!await _userManager.IsInRoleAsync(user, "SUPERADMIN"))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            
            Image userImage = user.Image;

            if (userUpdateDto.Photo != null)
            {
                var imageUpload = await _imageHelper.Upload(userUpdateDto.Email, userUpdateDto.Photo, ImageType.User);
                Image image = new(imageUpload.FullName, userUpdateDto.Photo.ContentType, userUpdateDto.Email);
                await _unitOfWork.GetRepository<Image>().AddAsync(image);
                await _unitOfWork.SaveAsync();

                _imageHelper.Delete(userImage.FileName);

                userImage = image;
            }

            //_mapper.Map(userUpdateDto, user);

            user.ImageId = userImage.Id;
            user.FirstName = userUpdateDto.FirstName;
            user.LastName = userUpdateDto.LastName;
            user.Email = userUpdateDto.Email;
            user.PhoneNumber = userUpdateDto.PhoneNumber;

            user.UserName = userUpdateDto.Email;
            user.NormalizedEmail = user.Email.ToUpper();
            user.NormalizedUserName = user.UserName.ToUpper();
            user.SecurityStamp = Guid.NewGuid().ToString();

            var findRole = await _roleManager.FindByIdAsync(userUpdateDto.RoleId.ToString());

            if(findRole.NormalizedName != "SUPERADMIN")
            {
                await _userManager.AddToRoleAsync(user, findRole.Name);
            }
           
            var updateResult = await _userManager.UpdateAsync(user);

            return (updateResult, userUpdateDto.Email);
        }

        public async Task<UserProfileDto> GetUserProfileAsync()
        {
            var userId = _user.GetLoggedInUserId();
            var getUserWithImage = await _unitOfWork.GetRepository<AppUser>().GetAsync(_ => _.Id == userId, _ => _.Image);
            var map = _mapper.Map<UserProfileDto>(getUserWithImage);
            map.Image.FileName = getUserWithImage.Image.FileName;

            return map;
        }

        private async Task<Guid> UploadImageForUserAsync(UserProfileDto userProfileDto)
        {
            var userEmail = _user.GetLoggedInEmail();

            var imageUpload = await _imageHelper.Upload($"{userProfileDto.FirstName}{userProfileDto.LastName}", userProfileDto.Photo, ImageType.User);
            Image image = new(imageUpload.FullName, userProfileDto.Photo.ContentType, userEmail);
            await _unitOfWork.GetRepository<Image>().AddAsync(image);

            return image.Id;
        }

        public async Task<bool> UserProfileUpdateAsync(UserProfileDto userProfileDto)
        {
            var userId = _user.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);

            var isVerified = await _userManager.CheckPasswordAsync(user, userProfileDto.CurrentPassword);
            if (isVerified && userProfileDto.NewPassword != null && userProfileDto.Photo != null)
            {

                var result = await _userManager.ChangePasswordAsync(user, userProfileDto.CurrentPassword, userProfileDto.NewPassword);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.PasswordSignInAsync(user, userProfileDto.NewPassword, true, false);

                    _mapper.Map(userProfileDto, user);
                    if (userProfileDto.Photo != null)
                    {
                        user.ImageId = await UploadImageForUserAsync(userProfileDto);
                    }

                    await _userManager.UpdateAsync(user);
                    await _unitOfWork.SaveAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (isVerified)
            {
                await _userManager.UpdateSecurityStampAsync(user);

                _mapper.Map(userProfileDto, user);
                if (userProfileDto.Photo != null)
                {
                    user.ImageId = await UploadImageForUserAsync(userProfileDto);
                }

                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveAsync();

                return true;
            }
            else
            {
                return false;
            }

        }
    
        public async Task<string> UndoDeleteAsync(Guid userId)
        {
            AppUser user;

            try
            {
                user = await _unitOfWork.GetRepository<AppUser>().GetAsync(_ => _.Id == userId);
            }
            catch
            {
                return null;
            }

            if(user != null)
            {
                user.IsDeleted = false;
                await _unitOfWork.SaveAsync();

                return user.Email;
            }

            return null;
        }
    }
}
