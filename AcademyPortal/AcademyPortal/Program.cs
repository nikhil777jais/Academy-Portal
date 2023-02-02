using AcademyPortal.Model;
using AcademyPortal.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddRazorPages().AddRazorRuntimeCompilation

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AcademyPortalDbContext>();

builder.Services.AddDbContext<AcademyPortalDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.ConfigureApplicationCookie(options =>   
   options.AccessDeniedPath = new PathString("/User/AccessDenied")
   );

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/user/signin";

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
