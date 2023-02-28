using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyPortalAPI.Controllers
{

    [Authorize(Roles = "Admin")]
    public class RoleController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;

        public RoleController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("addRole")]
        public async Task<ActionResult> AddRole(RoleDto roleDto)
        {

            //check if any role from same name is present in DB or not
            var role = await _uow.RoleRepository.GetRoleByNameAsync(roleDto.Name);
            if (role != null) return BadRequest(new { errors = $"{ roleDto.Name } is already present" });

            var result = await _uow.RoleRepository.AddRoleAsync(roleDto.Name);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = $"{ roleDto.Name} role added" });
        }
        
        [HttpGet("getRoles")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GerRole()
        {
            var roles = await _uow.RoleRepository.GetRoleDtosAsync();
            return Ok(roles);
        }

        [HttpDelete("deleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _uow.RoleRepository.GetRoleByIdAsync(id);
            if (role == null) return BadRequest(new { errors = $"Role with id {id} is not present" });

            var result = await _uow.RoleRepository.DeleteRoleAsync(role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = $"{ role.Name } role deleted" });
        }
    }
}
