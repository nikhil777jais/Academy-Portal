using AcademyPortal.Models;
using AcademyPortal.Repository.UnitOfWork;
using AcademyPortal.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModuleController : Controller
    {
        private readonly IUnitOfWork _uow;

        public ModuleController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Route("module", Name = "addModule")]
        public async Task<IActionResult> AddAndListModule()
        {
            ViewData["modules"] = await _uow.ModuleRepository.GetModulesWithUserAsync();
            return View("AddAndListModule");
        }

        [Route("module", Name = "addModule")]
        [HttpPost]
        public async Task<IActionResult> AddAndListModule(ModuleViewModel moduleViewModel)
        {
            var user = await _uow.UserRepository.GetUserByClaimsAsync(User);
            if (ModelState.IsValid)
            {
                await _uow.ModuleRepository.AddModuleAsync(user, moduleViewModel);
                if(await _uow.SaveChangesAsync()){
                    TempData["Message"] = $"Module Created Successfully !!";
                    TempData["Type"] = "success";
                }else{
                    TempData["Message"] = $"Error while saving module";
                    TempData["Type"] = "danger";
                }
            }
            return RedirectToRoute("addModule");
        }

        [Route("module/update/{id}", Name = "updateModule")]
        public async Task<IActionResult> UpdateModule(int id)
        {
            var module = await _uow.ModuleRepository.GetModuleByIdAsync(id);
            var moduleModel = new ModuleViewModel()
            {
                Name = module.Name,
                Technology = module.Technology,
                Proficiency = module.Proficiency
            };
            ViewData["module"] = module;
            return View("UpdateModule", moduleModel);
        }

        [Route("module/update/{id}", Name = "updateModule")]
        [HttpPost]
        public async Task<IActionResult> UpdateModule(int id, ModuleViewModel moduleViewModel)
        {
            var module = await _uow.ModuleRepository.GetModuleByIdAsync(id);
            if (ModelState.IsValid && module != null)
            {                
                module.Name = moduleViewModel.Name;
                module.Technology = moduleViewModel.Technology;
                module.Proficiency= moduleViewModel.Proficiency;
                _uow.ModuleRepository.UpdateModule(module);
                if(await _uow.SaveChangesAsync()){
                    TempData["Message"] = $"Module \"{module.Name}\" Updated Successfully !!";
                    TempData["Type"] = "success";   
                }else{
                    TempData["Message"] = $"Some Error while updating ";
                    TempData["Type"] = "danger";   
                }
            }
            ViewData["module"] = module;
            return View("UpdateModule");
        }


        [Route("module/delete/{id}", Name = "deleteModule")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var module = await _uow.ModuleRepository.GetModuleByIdAsync(id);
            if (module != null)
            {
                _uow.ModuleRepository.RemoveModule(module);
                if(await _uow.SaveChangesAsync()){
                    TempData["Message"] = $"Module Deleted Successfully !!";
                    TempData["Type"] = "success";  
                }else{
                    TempData["Message"] = $"Some Error while deleting";
                    TempData["Type"] = "danger";   
                } 
                return RedirectToRoute("addModule");
            }
            TempData["Message"] = $"Module not found!!";
            TempData["Type"] = "warning";
            return RedirectToRoute("addModule");
        }

        [Route("skillModuleMapping", Name = "skillModuleMapping")]
        public async Task<IActionResult> SkillModuleMapping()
        {
            var skills = await _uow.SkillRepository.GetSkillsWithModuleAsync();
            ViewData["skills"] = skills;
            return View("SkillModuleMapping");
        }
        
        [Route("addModulesToSkill/{id}", Name = "addModulesToSkill")]
        public async Task<IActionResult> AddModulesToSkill(int id)
        {
            var skill = await _uow.SkillRepository.GetSkillByIdWithModuleAsync(id);
            var modules = await _uow.ModuleRepository.GetModulesAsync();
            var listModules = new List<SelectListItem>();
            foreach(var item in modules)
            {
                //check if module if already present in skill or not
                listModules.Add(new SelectListItem(item.Name, item.Id.ToString(), skill.RelatedModules.Select(x => x.Id).Contains(item.Id)));
            }
            ViewBag.listModules = listModules;
            ViewData["skill"] = skill;
            return View("AddModulesToSkill");
        }

        [Route("addModulesToSkill/{id}", Name = "addModulesToSkill")]
        [HttpPost]
        public async Task<IActionResult> AddModulesToSkill(int id, SkillModuleMappingViewModel skillModuleMappingViewModel)
        {
            var skill = await _uow.SkillRepository.GetSkillByIdWithModuleAsync(id);
            if (ModelState.IsValid && skill != null)
            {
                var listModules = new List<Module>();
                
                foreach(var Id in skillModuleMappingViewModel.ModuleNames)  
                {
                    listModules.Add(await _uow.ModuleRepository.GetModuleByIdAsync(Convert.ToInt32(Id)));
                }
                skill.RelatedModules = listModules;
                _uow.SkillRepository.UpdateSkill(skill);
                if(await _uow.SaveChangesAsync()){
                    TempData["Message"] = $" Selected Modules added to {skill.Name} successfully!";
                    TempData["Type"] = "success";
                }else{
                    TempData["Message"] = $"Some error while adding modules";
                    TempData["Type"] = "danger";   
                }
            }
            return RedirectToRoute("addModulesToSkill");
        }

        [Route("{skillId}/removeModuleFromSkill/{id}", Name = "removeModuleFromSkill")]
        public async Task<IActionResult> RemoveModuleFromSkill(int id, int skillId)
        {
            var skill = await _uow.SkillRepository.GetSkillByIdWithModuleAsync(skillId);
            var module = await _uow.ModuleRepository.GetModulesByIdWithSkillsAsync(id);
            skill.RelatedModules.Remove(module);
            if(await _uow.SaveChangesAsync()){
                TempData["Message"] = $" Module {module.Name} is removed from {skill.Name} successfully!";
                TempData["Type"] = "success";
            }else{
                TempData["Message"] = $"Some error while removing module";
                TempData["Type"] = "danger";   
            }
            return RedirectToRoute("addModulesToSkill", new {Id = skill.Id });
        }
    }
}
