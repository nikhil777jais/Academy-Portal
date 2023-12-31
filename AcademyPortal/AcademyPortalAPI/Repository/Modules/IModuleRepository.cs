using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;

namespace AcademyPortalAPI.Repository.Modules
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetModulesAsync();
        Task<IEnumerable<Module>> GetModulesWithUserAsync();
        Task<IEnumerable<ModuleDto>> GetModuleDtosWithUserAsync();
        Task<IEnumerable<ModuleNameDto>> GetModuleNameDtosWithUserAsync();
        Task AddModuleAsync(ApplicationUser user, ModuleDto moduleDto);
        Task<Module> GetModuleByIdAsync(int id);
        Task<ModuleDto> GetModuleDtoByIdWithUserAsync(int id);
        void UpdateModule(Module module);
        void RemoveModule(Module module);
        Task<IEnumerable<Module>> GetModulesWithSkillsAsync();
        Task<bool> HasBatches(int id);
    }
}