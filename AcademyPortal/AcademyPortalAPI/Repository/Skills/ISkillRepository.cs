using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;

namespace AcademyPortalAPI.Repository.Skills
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> GetSkillsWithUserAsync();
        Task<IEnumerable<Skill>> GetSkillsAsync();
        Task<IEnumerable<SkillDto>> GetSkillDtosWithUserAsync();
        Task<IEnumerable<SkillNameDto>> GetSkillNameDtosWithUserAsync();
        Task<Skill> GetSkillByIdWithUserAsync(int id);
        Task<SkillDto> GetSkillDtoByIdWithUserAsync(int id);
        Task AddSkillAsync(ApplicationUser user, SkillDto skillDto); 
        void UpdateSkill(Skill skill);
        void RemoveSkill(Skill skill);
        Task<IEnumerable<Skill>> GetSkillsWithModuleAsync();
        Task<Skill> GetSkillByIdWithModuleAsync(int id);
        Task<Skill> GetSkillByIdAsync(int id);
        Task<bool> HasModules(int id);
        Task<bool> HasModulesWithId(int id, int moduleId);
        Task<bool> HasBatches(int id);

        Task<IEnumerable<ModuleSkillDto>> GetModuleSkillDtosAsync();
        Task<ModuleSkillDto> GetModuleSkillDtoByIdAsync(int id);

    }
}