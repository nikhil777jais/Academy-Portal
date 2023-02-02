using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Model;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortal.Extensions
{
    public static class IdentityServiceExtensions
    {   
        public static IServiceCollection IdentityConfig(this IServiceCollection services, IConfiguration config){
             
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>{
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AcademyPortalDbContext>();
            return services;
        }
    }
}