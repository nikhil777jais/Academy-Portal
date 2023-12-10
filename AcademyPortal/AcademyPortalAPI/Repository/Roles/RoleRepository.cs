using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortalAPI.Repository.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleRepository(RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IdentityResult> AddRoleAsync(string roleName)
        {
            var role = new ApplicationRole { Name = roleName };
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(ApplicationRole role)
        {
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<ApplicationRole> GetRoleByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<ApplicationRole> GetRoleByNameAsync(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<IEnumerable<RoleDto>> GetRoleDtosAsync()
        {
            return await _roleManager.Roles.ProjectTo<RoleDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationRole>> GetRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}