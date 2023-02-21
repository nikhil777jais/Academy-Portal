using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Models;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortal.Repository.Roles
{
    public interface IRoleRepository
    {
        Task<IEnumerable<ApplicationRole>> GetRolesAsync();
        Task<ApplicationRole> GetRoleByIdAsync(string id);
        Task<ApplicationRole> GetRoleByNameAsync(string name);
        Task<IdentityResult> AddRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(ApplicationRole role);
    }
}