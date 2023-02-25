using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NToastNotify;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Categories;
using YoutubeBlog.Entity.Models.DTOs.Roles;
using YoutubeBlog.Service.Services.Abstract;
using YoutubeBlog.Service.Services.Concrete;
using YoutubeBlog.Web.Const;
using YoutubeBlog.Web.ResultMessages;

namespace YoutubeBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConst.SuperAdmin}")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly IValidator<AppRole> _validator;
        private readonly IToastNotification _toast;

        public RoleController(IRoleService roleService, IMapper mapper, IValidator<AppRole> validator, IToastNotification toast)
        {
            _roleService = roleService;
            _mapper = mapper;
            _validator = validator;
            _toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetAllRolesAsync(_ => !_.IsDeleted);
            return View(roles);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RoleAddDto roleAddDto)
        {
            var map = _mapper.Map<AppRole>(roleAddDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var roleName = await _roleService.CreateRoleAsync(roleAddDto);

                if (roleName != null)
                {
                    _toast.AddSuccessToastMessage(Messages.Role.Add(roleName), new ToastrOptions { Title = Messages.ToastTitle.Success });
                }
                else
                {
                    _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.ToastTitle.Error });
                }

                return RedirectToAction("Index", "Role", new { Area = "Admin" });
            }

            result.AddToModelState(this.ModelState);
            return View(roleAddDto);
        }

        public async Task<IActionResult> Update(Guid roleId)
        {
            var role = await _roleService.GetRoleByGuidAsync(roleId);

            if (role != null)
            {
                var map = _mapper.Map<RoleUpdateDto>(role);
                return View(map);
            }

            return RedirectToAction("Index", "Home", new { Area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoleUpdateDto roleUpdateDto)
        {
            var map = _mapper.Map<AppRole>(roleUpdateDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var name = await _roleService.UpdateRoleAsync(roleUpdateDto);

                if (name != null)
                {
                    _toast.AddSuccessToastMessage(Messages.Role.Update(name), new ToastrOptions { Title = Messages.ToastTitle.Success });
                }
                else
                {
                    _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.ToastTitle.Error });
                }

                return RedirectToAction("Index", "Role", new { Area = "Admin" });
            }

            result.AddToModelState(this.ModelState);
            return View();
        }

        public async Task<IActionResult> Delete(Guid roleId)
        {
            var roleName = await _roleService.SafeDeleteRoleAsync(roleId);

            if (roleName != null)
            {
                _toast.AddSuccessToastMessage(Messages.Role.Delete(roleName), new ToastrOptions { Title = Messages.ToastTitle.Success });
            }
            else
            {
                _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.ToastTitle.Error });
            }

            return RedirectToAction("Index", "Role", new { Area = "Admin" });
        }
    }
}
