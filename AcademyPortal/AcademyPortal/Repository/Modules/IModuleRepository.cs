using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.DTOs;
using AcademyPortal.Models;

namespace AcademyPortal.Repository.Modules
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetModulesAsync();
        Task<IEnumerable<Module>> GetModulesWithUserAsync();
        Task AddModuleAsync(ApplicationUser user, ModuleDto moduleDto);
        Task<Module> GetModuleByIdAsync(int id);
        void UpdateModule(Module module);
        void RemoveModule(Module module);
        Task<IEnumerable<Module>> GetModulesWithSkillsAsync();
        Task<Module> GetModulesByIdWithSkillsAsync(int id);
    }
}