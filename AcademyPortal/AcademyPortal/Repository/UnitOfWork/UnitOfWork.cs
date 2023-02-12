using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Models;
using AcademyPortal.Repository.AllStatus;
using AcademyPortal.Repository.Batches;
using AcademyPortal.Repository.BatchUsers;
using AcademyPortal.Repository.Modules;
using AcademyPortal.Repository.Roles;
using AcademyPortal.Repository.Skills;
using AcademyPortal.Repository.User;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortal.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        //single instance of these properties is available application 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AcademyPortalDbContext _context;

        public UnitOfWork(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> singInManager, 
            RoleManager<IdentityRole> roleManager, 
            AcademyPortalDbContext context)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _context = context;
            _roleManager = roleManager;
        }

        //this instances are available where where uwo is injected 
        public IUserRepository UserRepository => new UserRepository(_userManager, _singInManager, _context);

        public IRoleRepository RoleRepository => new RoleRepository(_roleManager);

        public IStatusRepository StatusRepository => new StatusRepository(_context);

        public ISkillRepository SkillRepository => new SkillRepository(_context);

        public IModuleRepository ModuleRepository => new ModuleRepository(_context);

        public IBatchRepository BatchRepository =>  new BatchRepository(_context, _userManager);

        public IBatchUserRepository BatchUserRepository => new BatchUserRepository(_context);

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}