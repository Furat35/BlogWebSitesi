using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Users;
using YoutubeBlog.Service.Extensions;
using YoutubeBlog.Service.Services.Abstract;
using YoutubeBlog.Web.Const;
using YoutubeBlog.Web.ResultMessages;

namespace YoutubeBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<AppUser> _validator;
        private readonly IToastNotification _toast;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public AuthController(SignInManager<AppUser> signInManager, IUserService userService,IMapper mapper, IValidator<AppUser> validator, 
            IToastNotification toast, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _userService = userService;
            _mapper = mapper;
            _validator = validator;
            _toast = toast;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return (User.Identity != null && !User.Identity.IsAuthenticated)
                ? View() 
                : RedirectToAction("Index", "Home", new { Area = "" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetAppUserByEmailAsync(userLoginDto.Email != null ? userLoginDto.Email : string.Empty);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, (userLoginDto.Password == null ? "" : userLoginDto.Password), userLoginDto.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home", new { Area = "Admin" });
                    }
                }

                ModelState.AddModelError("", "E-post adresiniz veya şifreniz yanlıştır.");
                return View(userLoginDto);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            
        }

        [HttpGet]
        [Authorize(Roles = $"{RoleConst.SuperAdmin},{RoleConst.Admin},{RoleConst.User}")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            var user = await _userService.GetAppUserByEmailAsync(userRegisterDto.Email ?? String.Empty);

            if (user != null)
            {
                ModelState.AddModelError("", "Farklı bir email adresi ile üye olunuz.");
            }

            if (userRegisterDto.Password == null)
            {
                ModelState.AddModelError("Password", "Bu alanı boş bırakmayınız.");
            }

            user = _mapper.Map<AppUser>(userRegisterDto);
            var result = await _validator.ValidateAsync(user);

            if (result.IsValid)
            {
                var map = _mapper.Map<UserAddDto>(userRegisterDto);
                var createResult = await _userService.CreateUserAsync(map);

                if (createResult.Succeeded)
                {
                    _toast.AddSuccessToastMessage("Başarıyla kayıt olundu.", new ToastrOptions { Title = Messages.ToastTitle.Success });
                    return RedirectToAction("Index", "Home", new { Area = "" });
                }
                else
                {
                    ValidationExtensions.AddToIdentityModelState(createResult, this.ModelState);
                    return View(userRegisterDto);
                }
            }

            ValidationExtensions.AddToModelState(result ,this.ModelState);

            return View(userRegisterDto);
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
    }
}
