using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AcademyPortalAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SkillController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;

        public SkillController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("getSkills")]
        public async Task<IActionResult> GetSkills()
        {
            var skills = await _uow.SkillRepository.GetSkillDtosWithUserAsync();

            if(skills == null) return NotFound(new { errors = $"skills not found" });
            return Ok(skills);
        }

        [HttpGet("getSkill/{id}")]
        public async Task<IActionResult> GetSkillById(int id)
        {
            var skill = await _uow.SkillRepository.GetSkillDtoByIdWithUserAsync(id);

            if(skill == null) return NotFound(new { errors = $"skill not found" });

            return Ok(skill);
        }

        [HttpPost("addSkill")]
        public async Task<IActionResult> AddSkill(SkillDto skillDto)
        {
            var user = await _uow.UserRepository.GetUserByClaimsAsync(User);

            await _uow.SkillRepository.AddSkillAsync(user, skillDto);
            if (!await _uow.SaveChangesAsync()) return BadRequest(new { errors = $"Unable to add skill" });
            
            return Ok(new { message = $"{skillDto.Name} skill added" });
        }
        
        [HttpPatch("updateSkill/{id}")]
        public async Task<IActionResult> UpdateSkill(SkillDto skillDto, int id)
        {

            var skill = await _uow.SkillRepository.GetSkillByIdWithUserAsync(id);

            if (skill == null) return BadRequest(new { error = $"skill not found with id {id}" });

            skill.Name = skillDto.Name;
            skill.Family = skillDto.Family;
            _uow.SkillRepository.UpdateSkill(skill);

            if (!await _uow.SaveChangesAsync()) return BadRequest(new { error = $"Unable to update skill" });
            return Ok(new { message = $"skill with id -> {id} updated" });
        }

        [HttpDelete("deleteSkill/{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {

            var skill = await _uow.SkillRepository.GetSkillByIdWithUserAsync(id);

            var hasMod = await _uow.SkillRepository.HasModules(id);
            if (hasMod) return BadRequest(new { error = $"can not delete due to Referential Integrity" });

            var hasBatches = await _uow.SkillRepository.HasBatches(id);
            if (hasBatches) return BadRequest(new { error = $"can not delete due to Referential Integrity"});

            if (skill == null) return NotFound(new { error = $"skill not found with id {id}" });

            _uow.SkillRepository.RemoveSkill(skill);
            if (!await _uow.SaveChangesAsync()) return BadRequest(new { error = $"Unable to delete skill" });

            return Ok(new { message = $"skill with {id} deleted" });
        }
    }
}
