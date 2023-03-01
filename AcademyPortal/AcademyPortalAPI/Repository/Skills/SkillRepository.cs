using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortalAPI.Repository.Skills
{
    public class SkillRepository : ISkillRepository
    {
        private readonly AcademyPortalDbContext _db;
        private readonly IMapper _mapper;

        public SkillRepository(AcademyPortalDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task AddSkillAsync(ApplicationUser user, SkillDto skillDto)
        {
            var skill = new Skill()
            {
                Name = skillDto.Name,
                Family = skillDto.Family,
                CreatedBy = user
            };
            await _db.Skills.AddAsync(skill);
        }

        public async Task<Skill> GetSkillByIdWithUserAsync(int id)
        {
            var skill = await _db.Skills.Include(s => s.CreatedBy).FirstOrDefaultAsync(s => s.Id == id);
            return skill;                 
        }

        public async Task<IEnumerable<Skill>> GetSkillsAsync()
        {
            return await _db.Skills.ToListAsync();
        }
        public async Task<IEnumerable<Skill>> GetSkillsWithUserAsync()
        {
            return await _db.Skills.Include(s => s.CreatedBy).ToListAsync();
        }

        public void RemoveSkill(Skill skill)
        {
            _db.Skills.Remove(skill);
        }

        public void UpdateSkill(Skill skill)
        {
            _db.Skills.Update(skill);
        }

        public async Task<IEnumerable<Skill>> GetSkillsWithModuleAsync()
        {
            return await _db.Skills.Include(s => s.CreatedBy).Include(s => s.RelatedModules).ToListAsync();
        }

        public async Task<Skill> GetSkillByIdWithModuleAsync(int id)
        {
            var skill =  await _db.Skills.Include(s => s.CreatedBy).Include(s => s.RelatedModules).SingleOrDefaultAsync(s => s.Id == id);  
            return skill;                            
        }

        public async Task<IEnumerable<SkillDto>> GetSkillDtosWithUserAsync()
        {
            var query = _db.Skills.Include(s => s.CreatedBy).AsQueryable();
            return await query.ProjectTo<SkillDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<SkillDto> GetSkillDtoByIdWithUserAsync(int id)
        {
            var query = _db.Skills.Include(s => s.CreatedBy).Where(s => s.Id == id).AsQueryable();
            return await query.ProjectTo<SkillDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<bool> HasModules(int id)
        {
            var modules = _db.Modules.Include(m => m.RelatedSkills).AsQueryable();
            var isPresent = await modules.AnyAsync(m => m.RelatedSkills.Any(s => s.Id == id));
            return isPresent;
        }

        public async Task<bool> HasBatches(int id)
        {
            var batches = _db.Batches.Include(b => b.RelaedSkill).AsQueryable();
            var isPresent = await batches.AnyAsync(b => b.RelaedSkill.Id == id);
            return isPresent;
        }
    }
}