using BackendProject.DAL;
using BackendProject.Models;
using BackendProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProjectDbContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
    );

builder.Services.AddScoped<LayoutService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<ProjectDbContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToAccessDenied = options.Events.OnRedirectToLogin = context =>
    {
        var uri = new Uri(context.RedirectUri);
        if (context.HttpContext.Request.Path.Value.StartsWith("/manage"))
        {
            context.Response.Redirect("/manage/account/login" + uri.Query);
        }
        else
        {
            context.Response.Redirect("/account/login" + uri.Query);
        }
        return Task.CompletedTask;
    };
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=home}/{action=index}/{id?}"); 

app.UseStaticFiles();
app.Run();
