using AcademyPortal.Extensions;
using AcademyPortal.Middleware;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

// Application Services
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.IdentityConfig(builder.Configuration);

builder.Services.AddCors();

builder.Services.ConfigureApplicationCookie(options =>{
    options.LoginPath = "/user/signIn";
    options.AccessDeniedPath = new PathString("/User/AccessDenied");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
