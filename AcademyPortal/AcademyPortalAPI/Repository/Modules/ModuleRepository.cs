using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortalAPI.Repository.Modules
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly AcademyPortalDbContext _db;
        private readonly IMapper _mapper;

        public ModuleRepository(AcademyPortalDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task AddModuleAsync(ApplicationUser user, ModuleDto moduleDto)
        {
            var module = new Module()
            {
                Name = moduleDto.Name,
                Technology = moduleDto.Technology,
                Proficiency= moduleDto.Proficiency,
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

        public async Task<Module> GetModuleByIdWithUserAsync(int id)
        {
            var module = await _db.Modules.Include(m => m.CreatedBy).SingleOrDefaultAsync(m => m.Id == id);
            return module;
        }

        public async Task<IEnumerable<Module>> GetModulesWithUserAsync()
        {
            return await _db.Modules.Include(m => m.CreatedBy).ToListAsync();
        }
        
        public async Task<IEnumerable<Module>> GetModulesAsync()
        {
            return await _db.Modules.ToListAsync();
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

        public async Task<IEnumerable<ModuleDto>> GetModuleDtosWithUserAsync()
        {
            var query = _db.Modules.Include(m => m.CreatedBy).AsQueryable();
            return await query.ProjectTo<ModuleDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<ModuleDto> GetModuleDtoByIdWithUserAsync(int id)
        {
            var query = _db.Modules.Include(m => m.CreatedBy).Where(m => m.Id == id).AsQueryable();
            return await query.ProjectTo<ModuleDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<bool> HasBatches(int id)
        {
            var batches = _db.Batches.Include(b => b.RelaedModule).AsQueryable();
            var isPresent = await batches.AnyAsync(b => b.RelaedModule.Id == id);
            return isPresent;
        }
    }
}