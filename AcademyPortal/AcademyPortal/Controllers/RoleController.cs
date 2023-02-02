using AcademyPortal.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademyPortal.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [Route("role/add", Name = "AddAndListRole")]
        public async Task<IActionResult> AddAndListRole()
        {
            var roles = _roleManager.Roles.ToList();
            ViewData["roles"] = roles;
            return View("AddRole");
        }

        [Route("role/add", Name = "AddAndListRole")]
        [HttpPost]
        public async Task<IActionResult> AddAndListRole(RoleViewModel roleViewModel)
        {
            var role = new IdentityRole { Name = roleViewModel.Name };
            if (ModelState.IsValid)
            {
                if (await _roleManager.FindByNameAsync(role.Name) == null)
                {
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Role added Successfully !!";
                        TempData["Type"] = "success";
                        return RedirectToRoute("AddAndListRole");
                    }
                }
                TempData["Message"] = $"{roleViewModel.Name} Is already in the databse";
                TempData["Type"] = "danger";
            }
            var roles = _roleManager.Roles.ToList();
            ViewData["roles"] = roles;
            return View("AddRole");
        }

        [Route("role/delete/{Id}", Name = "DeleteRole")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = _roleManager.Roles.FirstOrDefault(r => r.Id == Id);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
            TempData["Message"] = "Role Deleted Successfully !!";
            TempData["Type"] = "success";
            return RedirectToRoute("AddAndListRole");
        }

    }
}
