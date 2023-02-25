using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Users;
using YoutubeBlog.Service.Extensions;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Web.Areas.Admin.ViewComponents
{
    public class DashboardHeaderViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public DashboardHeaderViewComponent(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = LogedInUserExtensions.GetLoggedInUserId(HttpContext.User);
            var loggedInUser = await _userService.GetAppUserByIdIncludeImageAsync(userId);

            var map =  _mapper.Map<UserDto>(loggedInUser);

            var role = await _userService.GetUserRoleAsync(loggedInUser);
            map.Role = role;

            return View(map);
        }
    }
}
