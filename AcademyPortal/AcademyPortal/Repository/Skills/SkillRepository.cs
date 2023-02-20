using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.DTOs;
using AcademyPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortal.Repository.Skills
{
    public class SkillRepository : ISkillRepository
    {
        private readonly AcademyPortalDbContext _db;

        public SkillRepository(AcademyPortalDbContext db)
        {
            _db = db;
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

        public async Task<Skill> GetSkillByIdAsync(int id)
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
    }
}