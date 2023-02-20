using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.DTOs;
using AcademyPortal.Models;

namespace AcademyPortal.Repository.Skills
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> GetSkillsWithUserAsync();
        Task<IEnumerable<Skill>> GetSkillsAsync();
        Task<Skill> GetSkillByIdAsync(int id);
        Task AddSkillAsync(ApplicationUser user, SkillDto skillDto); 
        void UpdateSkill(Skill skill);
        void RemoveSkill(Skill skill);
        Task<IEnumerable<Skill>> GetSkillsWithModuleAsync();
        Task<Skill> GetSkillByIdWithModuleAsync(int id);
    }
}