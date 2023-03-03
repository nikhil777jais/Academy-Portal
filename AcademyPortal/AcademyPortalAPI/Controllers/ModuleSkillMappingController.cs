using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;
using AcademyPortalAPI.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyPortalAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModuleSkillMappingController : BaseAPIController 
    {
        private readonly IUnitOfWork _uow;

        public ModuleSkillMappingController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("getMappingList")]
        public async Task<IActionResult> GetMappingList()
        {
            var moduleSkills = await _uow.SkillRepository.GetModuleSkillDtosAsync();
            return Ok(moduleSkills);
        }
        
        [HttpGet("getMapping/{id}")]
        public async Task<IActionResult> GetMappingById(int id)
        {
            var moduleSkill = await _uow.SkillRepository.GetModuleSkillDtoByIdAsync(id);
            return Ok(moduleSkill);
        }
        
        [HttpPost("addModules/{id}")]
        public async Task<IActionResult> AddModules(int id, SkillModuleMappingDto skillModuleMappingDto)
        {
            var skill = await _uow.SkillRepository.GetSkillByIdWithModuleAsync(id);
            if (skill == null) return NotFound(new { error = $"Skill is not present with id -> {id}"});

            foreach (var moduleId in skillModuleMappingDto.ModuleIds)
            {

                if(await _uow.SkillRepository.HasModulesWithId(id, moduleId)) continue;

                Module module = await _uow.ModuleRepository.GetModuleByIdAsync(moduleId);
                
                if (module == null) return NotFound(new { error = $"module is not present with id -> {moduleId}" }) ;

                skill.RelatedModules.Add(module);
            }

            _uow.SkillRepository.UpdateSkill(skill);
            if (!await _uow.SaveChangesAsync()) BadRequest(new { error = $"unable to add modules" });
            
            return Ok(new { message = $"modules are added to skill with id -> { id }" });
        }

        [HttpDelete("{skillId}/removeModuleFromSkill/{id}")]
        public async Task<IActionResult> DeleteModule(int id, int skillId)
        {
            var skill = await _uow.SkillRepository.GetSkillByIdWithModuleAsync(skillId);
            if (skill == null) return NotFound(new { error = $"Skill is not present with id -> {skillId}" });

            //Here we have to check weather the module belong to skill or not 
            if (!await _uow.SkillRepository.HasModulesWithId(skillId, id))
                return NotFound(new { error = $"Module is not present with id -> {id}" });
            
            var module = await _uow.ModuleRepository.GetModuleByIdAsync(id);
            if (module == null) return NotFound(new { error = $"Module is not present with id -> {id}" });

            skill.RelatedModules.Remove(module);
            if (!await _uow.SaveChangesAsync()) BadRequest(new { error = $"unable to delete module with id -> {id}" });

            return Ok(new { message = $"modules are removed from skill with id -> {skillId}" });
        }
    }
}
