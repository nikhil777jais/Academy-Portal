using AcademyPortal.DTOs;
using AcademyPortal.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademyPortal.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly IUnitOfWork _uow;

        public RoleController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Route("role/add", Name = "AddAndListRole")]
        public async Task<IActionResult> AddAndListRole()
        {
            var roles = await _uow.RoleRepository.GetRolesAsync();
            ViewData["roles"] = roles;
            return View("AddRole");
        }

        [Route("role/add", Name = "AddAndListRole")]
        [HttpPost]
        public async Task<IActionResult> AddAndListRole(RoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                //check if any role from same name is present in DB or not
                var role = await _uow.RoleRepository.GetRoleByNameAsync(roleDto.Name);
                if ( role == null)
                {
                    var result = await _uow.RoleRepository.AddRoleAsync(roleDto.Name);
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Role added Successfully !!";
                        TempData["Type"] = "success";
                        return RedirectToRoute("AddAndListRole");
                    }
                }
                TempData["Message"] = $"{roleDto.Name} Is already in the database";
                TempData["Type"] = "danger";
            }
            return RedirectToRoute("AddAndListRole");
        }

        [Route("role/delete/{id}", Name = "DeleteRole")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _uow.RoleRepository.GetRoleByIdAsync(id);
            if (role != null)
            {
                var result  = await _uow.RoleRepository.DeleteRoleAsync(role);
                if(result.Succeeded){
                    TempData["Message"] = "Role Deleted Successfully !!";
                    TempData["Type"] = "success";
                    return RedirectToRoute("AddAndListRole");
                }
            }
            TempData["Message"] = "Role not found";
            TempData["Type"] = "danger";
            return RedirectToRoute("AddAndListRole");
        }
    }
}
