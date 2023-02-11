using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Models;
using AcademyPortal.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortal.Repository.Modules
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly AcademyPortalDbContext _db;

        public ModuleRepository(AcademyPortalDbContext db)
        {
            _db = db;
        }

        public async Task AddModuleAsync(ApplicationUser user, ModuleViewModel moduleViewModel)
        {
            var module = new Module()
            {
                Name = moduleViewModel.Name,
                Technology = moduleViewModel.Technology,
                Proficiency= moduleViewModel.Proficiency,
                CreatedBy = user
            };
            await _db.Modules.AddAsync(module);
        }

        public void UpdateModule(Module module)
        {
            _db.Modules.Update(module);
        }

        public void RemoveModule(Module module)
        {
            _db.Modules.Remove(module);
        }

        public async Task<Module> GetModuleByIdAsync(int id)
        {
            var module = await _db.Modules.Include(m => m.CreatedBy).SingleOrDefaultAsync(m => m.Id == id);
            return module;
        }

        public async Task<IEnumerable<Module>> GetModulesAsync()
        {
            return await _db.Modules.Include(m => m.CreatedBy).ToListAsync();
        }

        public async Task<Module> GetModulesByIdWithSkillsAsync(int id)
        {
            var module = await _db.Modules.Include(m => m.CreatedBy).Include(m => m.RelatedSkills).SingleOrDefaultAsync(m => m.Id == id);
            return module;                                    
        }

        public async Task<IEnumerable<Module>> GetModulesWithSkillsAsync()
        {
            return await _db.Modules.Include(m => m.CreatedBy).Include(m => m.RelatedSkills).ToListAsync();
        }

    }
}