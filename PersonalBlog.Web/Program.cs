using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using YoutubeBlog.Data.Context;
using YoutubeBlog.Data.Extensions;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Service.Describers;
using YoutubeBlog.Service.Extensions;
using YoutubeBlog.Service.Services.Abstract;
using YoutubeBlog.Service.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions
    {
        PositionClass = ToastPositions.TopRight,
        TimeOut = 3000
    });
builder.Services.LoadDataLayerExtension(builder.Configuration);
builder.Services.LoadServiceLayerExtension();
// Add services to the container.

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
}).AddRoleManager<RoleManager<AppRole>>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders()
.AddErrorDescriber<CustomIdentityErrorDescriber>(); 

builder.Services.AddSession();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = new PathString("/Admin/Auth/Login");
    config.LogoutPath = new PathString("/Admin/Auth/Logout");
    config.Cookie = new CookieBuilder
    {
        Name = "YoutubeBlog",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest,
    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromDays(7);
    config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseNToastNotify();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
