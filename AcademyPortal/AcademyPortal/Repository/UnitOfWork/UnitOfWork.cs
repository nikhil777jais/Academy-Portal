using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Model;
using AcademyPortal.Repository.User;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortal.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        //single instance of these properties is available application 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private readonly AcademyPortalDbContext _context;

        public UnitOfWork(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager, AcademyPortalDbContext context)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _context = context;
        }

        //this instances are available where where uwo is injected 
        public IUserRepository UserRepository => new UserRepository(_userManager, _singInManager, _context);

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}