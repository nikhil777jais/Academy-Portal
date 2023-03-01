using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.Models;
using AcademyPortalAPI.Repository.AllStatus;
using AcademyPortalAPI.Repository.Batches;
using AcademyPortalAPI.Repository.BatchUsers;
using AcademyPortalAPI.Repository.Modules;
using AcademyPortalAPI.Repository.Roles;
using AcademyPortalAPI.Repository.Skills;
using AcademyPortalAPI.Repository.User;
using AutoMapper;
using AcademyPortalAPI.Services.Token;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortalAPI.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        //single instance of these properties is available application 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly AcademyPortalDbContext _context;

        public UnitOfWork(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> singInManager, 
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IMapper mapper,
            AcademyPortalDbContext context)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _context = context;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        //this instances are available where where uwo is injected 
        public IUserRepository UserRepository => new UserRepository(_userManager, _singInManager, _context, _mapper);

        public IRoleRepository RoleRepository => new RoleRepository(_roleManager, _mapper);

        public IStatusRepository StatusRepository => new StatusRepository(_context, _mapper);

        public ISkillRepository SkillRepository => new SkillRepository(_context, _mapper);

        public IModuleRepository ModuleRepository => new ModuleRepository(_context, _mapper);

        public IBatchRepository BatchRepository =>  new BatchRepository(_context, _userManager);

        public IBatchUserRepository BatchUserRepository => new BatchUserRepository(_context);

        public ITokenService TokenService => new TokenService(_configuration, _userManager);

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}