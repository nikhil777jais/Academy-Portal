using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortalAPI.Repository.Roles
{
    public interface IRoleRepository
    {
        Task<IEnumerable<ApplicationRole>> GetRolesAsync();
        Task<IEnumerable<RoleDto>> GetRoleDtosAsync();
        Task<ApplicationRole> GetRoleByIdAsync(string id);
        Task<ApplicationRole> GetRoleByNameAsync(string name);
        Task<IdentityResult> AddRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(ApplicationRole role);
    }
}