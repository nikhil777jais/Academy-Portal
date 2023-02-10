using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Models;
using AcademyPortal.ViewModel;
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
        public async Task AddSkillAsync(ApplicationUser user, SkillViewModel skillViewModel)
        {
            var skill = new Skill()
            {
                Name = skillViewModel.Name,
                Family = skillViewModel.Family,
                CreatedBy = user
            };
            await _db.Skills.AddAsync(skill);
        }

        public async Task<Skill> GetSkillByIdAsync(int id)
        {
            return await _db.Skills.Include(s => s.CreatedBy).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Skill>> GetSkillsAsync()
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
    }
}