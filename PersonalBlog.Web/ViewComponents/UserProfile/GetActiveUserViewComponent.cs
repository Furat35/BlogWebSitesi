using Microsoft.AspNetCore.Mvc;
using YoutubeBlog.Service.Extensions;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Web.ViewComponents.UserProfile
{
    public class GetActiveUserViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public GetActiveUserViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = LogedInUserExtensions.GetLoggedInUserId(HttpContext.User);
            var user = await _userService.GetAppUserByIdAsync(userId);
            object userInfo = user.FirstName + " "+ user.LastName;

            return View(userInfo);
        }
    }
}
