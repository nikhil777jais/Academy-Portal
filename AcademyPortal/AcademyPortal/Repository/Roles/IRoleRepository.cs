using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortal.Repository.Roles
{
    public interface IRoleRepository
    {
        Task<IEnumerable<IdentityRole>> GetRolesAsync();
        Task<IdentityRole> GetRoleByIdAsync(string id);
        Task<IdentityRole> GetRoleByNameAsync(string name);
        Task<IdentityResult> AddRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(IdentityRole role);
    }
}