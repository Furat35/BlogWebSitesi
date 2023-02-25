using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YoutubeBlog.Web.Const;

namespace YoutubeBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConst.SuperAdmin}")]
    public class SmtpSettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
