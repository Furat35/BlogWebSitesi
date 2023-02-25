using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Users;
using YoutubeBlog.Service.Extensions;
using YoutubeBlog.Service.Services.Abstract;
using YoutubeBlog.Web.Const;
using YoutubeBlog.Web.ResultMessages;
using static YoutubeBlog.Web.ResultMessages.Messages;

namespace YoutubeBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IValidator<AppUser> _validator;
        private readonly IValidator<UserAddDto> _userAddDtoValidator;
        private readonly IToastNotification _toast;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IMapper mapper, IValidator<AppUser> validator, IValidator<UserAddDto> userAddDtoValidator, IToastNotification toast, 
            IUserService userService, IRoleService roleService)
        {
            _mapper = mapper;
            _validator = validator;
            _userAddDtoValidator = userAddDtoValidator;
            _toast = toast;
            _userService = userService;
            _roleService = roleService;
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin},{RoleConst.User}")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersWithRoleAsync(_ => !_.IsDeleted);
            return View(users);
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Add()
        {
            var roles = await _roleService.GetAllRolesAsync(_ => !_.IsDeleted);
            return View(new UserAddDto { Roles = roles });
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            var result = await _userAddDtoValidator.ValidateAsync(userAddDto);

            if (result.IsValid)
            {
                var map = _mapper.Map<AppUser>(userAddDto);
                map.UserName = userAddDto.Email;
                var createResult = await _userService.CreateUserAsync(userAddDto);

                if (createResult.Succeeded)
                {
                    _toast.AddSuccessToastMessage(Messages.User.Add(map.Email), new ToastrOptions { Title = ToastTitle.Success });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    createResult.AddToIdentityModelState(this.ModelState);
                    return View(userAddDto);
                }
            }

            result.AddToModelState(this.ModelState);
            userAddDto.Roles = await _roleService.GetAllRolesAsync(_ => !_.IsDeleted);

            return View(userAddDto);
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await _userService.GetAppUserByIdIncludeImageAsync(userId);

            if (user != null)
            {
                var map = _mapper.Map<UserUpdateDto>(user);
                var userRoleId = await _roleService.GetRoleGuidAsync(await _userService.GetUserRoleAsync(user));

                if (userRoleId != null)
                {
                    map.RoleId = Guid.Parse(userRoleId);
                    map.Roles = await _roleService.GetAllRolesAsync(_ => !_.IsDeleted); 

                    return View(map);
                }

                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }

            return NotFound();
        }


        [HttpPost]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            var map = _mapper.Map<AppUser>(userUpdateDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var updateResult = await _userService.UpdateUserAsync(userUpdateDto);

                if (updateResult.identityResult.Succeeded)
                {
                    _toast.AddSuccessToastMessage(Messages.User.Update(updateResult.userName), new ToastrOptions { Title = Messages.ToastTitle.Success });
                }
                else
                {
                    _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.ToastTitle.Error });
                }

                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }

            return View(userUpdateDto);
        }
            
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await _userService.SafeDeleteUserAsync(userId);

            if (result.identityResult != null)
            {
                if (result.identityResult.Succeeded)
                {
                    _toast.AddSuccessToastMessage(Messages.User.Delete(result.email), new ToastrOptions { Title = ToastTitle.Success });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    result.identityResult.AddToIdentityModelState(this.ModelState);
                    return View(userId);
                }
            }

            return NotFound();
        }

        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Profile()
        {
            var profile = await _userService.GetUserProfileAsync();
            return View(profile);
        }

        [HttpPost]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin}")]
        public async Task<IActionResult> Profile(UserProfileDto userProfileDto)
        {
            Guid userId = LogedInUserExtensions.GetLoggedInUserId(HttpContext.User);
            var user = _userService.GetAppUserByIdAsync(userId);

            if (ModelState.IsValid)
            {
                var result = await _userService.UserProfileUpdateAsync(userProfileDto);

                if (result)
                {
                    _toast.AddSuccessToastMessage(Messages.GlobalMessage.Success, new ToastrOptions { Title = ToastTitle.Success });
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
                else
                {
                    var profile = await _userService.GetUserProfileAsync();
                    _toast.AddSuccessToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = ToastTitle.Error });

                    return View(profile);
                }
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> DeletedUser()
        {
            var users = await _userService.GetAllUsersWithRoleAsync(_ => _.IsDeleted);
            return View(users);
        }

        public async Task<IActionResult> UndoDelete(Guid userId)
        {
            var user = await _userService.UndoDeleteAsync(userId);

            if(user != null)
            {
                _toast.AddSuccessToastMessage(Messages.User.UndoDelete(user), new ToastrOptions { Title = Messages.ToastTitle.Success });
            }
            else
            {
                _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.ToastTitle.Error });
            }

            return RedirectToAction("DeletedUser", "User", new {Area = "Admin"});
        }
    }
}
