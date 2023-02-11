using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Models;
using AcademyPortal.ViewModel;

namespace AcademyPortal.Repository.Modules
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetModulesAsync();
        Task AddModuleAsync(ApplicationUser user, ModuleViewModel moduleViewModel);
        Task<Module> GetModuleByIdAsync(int id);
        void UpdateModule(Module module);
        void RemoveModule(Module module);
        Task<IEnumerable<Module>> GetModulesWithSkillsAsync();
        Task<Module> GetModulesByIdWithSkillsAsync(int id);
    }
}