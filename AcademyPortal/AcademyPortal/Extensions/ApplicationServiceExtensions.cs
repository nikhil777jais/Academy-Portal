using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Model;
using AcademyPortal.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortal.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config){
            
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddDbContext<AcademyPortalDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddTransient<IUserRepository, UserRepository>();
            services.ConfigureApplicationCookie(
                options => options.AccessDeniedPath = new PathString("/User/AccessDenied")
            );
            services.ConfigureApplicationCookie(options =>{
                options.LoginPath = "/user/signin";
            });
            return services;
        }
    }
}