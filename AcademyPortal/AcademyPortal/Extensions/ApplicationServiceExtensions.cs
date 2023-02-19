using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Models;
using AcademyPortal.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortal.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config){
            
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddDbContext<AcademyPortalDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.ConfigureApplicationCookie(
                options => options.AccessDeniedPath = new PathString("/User/AccessDenied")
            );
            return services;
        }
    }
}