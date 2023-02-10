using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Models;
using AcademyPortal.ViewModel;

namespace AcademyPortal.Repository.Skills
{
    public interface ISkillRepository
    {
        Task<IEnumerable<Skill>> GetSkillsAsync();
        Task<Skill> GetSkillByIdAsync(int id);
        Task AddSkillAsync(ApplicationUser user, SkillViewModel skillViewModel); 
        void UpdateSkill(Skill skill);
        void RemoveSkill(Skill skill);
    }
}