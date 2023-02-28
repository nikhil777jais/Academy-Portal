using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.Helpers;
using AcademyPortalAPI.Models;
using AcademyPortalAPI.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortalAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config){
            
            services.AddDbContext<AcademyPortalDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.ConfigureApplicationCookie(
                options => options.AccessDeniedPath = new PathString("/User/AccessDenied")
            );
            return services;
        }
    }
}