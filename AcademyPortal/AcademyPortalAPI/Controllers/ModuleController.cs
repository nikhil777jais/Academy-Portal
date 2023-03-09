using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyPortalAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModuleController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;

        public ModuleController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("getModules")]
        public async Task<IActionResult> GetModules()
        {
            var modules = await _uow.ModuleRepository.GetModuleDtosWithUserAsync();

            if (modules == null) return NotFound(new { errors = $"modules not found" });
            return Ok(modules);
        }
        
        [HttpGet("getModuleNames")]
        public async Task<IActionResult> GetModuleNames()
        {
            var modules = await _uow.ModuleRepository.GetModuleNameDtosWithUserAsync();

            if (modules == null) return NotFound(new { errors = $"modules not found" });
            return Ok(modules);
        }
            
        [HttpGet("getModule/{id}")]
        public async Task<IActionResult> GetModuleById(int id)
        {
            var module = await _uow.ModuleRepository.GetModuleDtoByIdWithUserAsync(id);

            if (module == null) return NotFound(new { errors = $"module not found" });

            return Ok(module);
        }

        [HttpPost("addModule")]
        public async Task<IActionResult> AddModule(ModuleDto moduleDto)
        {
            var user = await _uow.UserRepository.GetUserByClaimsAsync(User);

            await _uow.ModuleRepository.AddModuleAsync(user, moduleDto);
            if (!await _uow.SaveChangesAsync()) return BadRequest(new { errors = $"Unable to add module" });

            return Ok(new { message = $"{moduleDto.Name} module added" });
        }

        [HttpPatch("updateModule/{id}")]
        public async Task<IActionResult> UpdateModule(ModuleDto moduleDto, int id)
        {

            var module = await _uow.ModuleRepository.GetModuleByIdAsync(id);

            if (module == null) return BadRequest(new { error = $"module not found with id {id}" });

            module.Name = moduleDto.Name;
            module.Technology = moduleDto.Technology;
            module.Proficiency = moduleDto.Proficiency;

            _uow.ModuleRepository.UpdateModule(module);

            if (!await _uow.SaveChangesAsync()) return BadRequest(new { error = $"Unable to update module" });
            return Ok(new { message = $"module with id -> {id} updated" });
        }

        [HttpDelete("deleteModule/{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {

            var module = await _uow.ModuleRepository.GetModuleByIdAsync(id);

            var hasBatches = await _uow.ModuleRepository.HasBatches(id);
            if (hasBatches) return BadRequest(new { error = $"can not delete due to Referential Integrity" });

            if (module == null) return NotFound(new { error = $"module not found with id {id}" });

            _uow.ModuleRepository.RemoveModule(module);
            if (!await _uow.SaveChangesAsync()) return BadRequest(new { error = $"Unable to delete module" });

            return Ok(new { message = $"module with {id} deleted" });
        }

    }
}
