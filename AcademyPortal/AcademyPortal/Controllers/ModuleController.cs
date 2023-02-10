using AcademyPortal.Models;
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
        private readonly AcademyPortalDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ModuleController(AcademyPortalDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Route("module", Name = "addmodule")]
        public async Task<IActionResult> AddAndListModule()
        {
            var module = await _db.Modules.Include(s => s.CreatedBy).ToListAsync();
            ViewData["modules"] = await _db.Modules.Include(s => s.CreatedBy).ToListAsync();
            return View("AddAndListModule");
        }

        [Route("module", Name = "addmodule")]
        [HttpPost]
        public async Task<IActionResult> AddAndListModule(ModuleViewModel moduleViewModel)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (ModelState.IsValid)
            {
                var moduleobj = new Module()
                {
                    Name = moduleViewModel.Name,
                    Technology = moduleViewModel.Technology,
                    Proficiency= moduleViewModel.Proficiency,
                    CreatedBy = user
                };
                await _db.Modules.AddAsync(moduleobj);
                await _db.SaveChangesAsync();
                TempData["Message"] = $"Module Created Successfully !!";
                TempData["Type"] = "success";
                return RedirectToRoute("addmodule");
            }
            ViewData["modules"] = await _db.Modules.Include(s => s.CreatedBy).ToListAsync();
            return View("AddAndListModule");
        }

        [Route("module/update/{Id}", Name = "updatemodule")]
        public async Task<IActionResult> UpdateModule(int Id)
        {
            var moduleobj = await _db.Modules.Where(sk => sk.Id == Id).Include(sk => sk.CreatedBy).SingleOrDefaultAsync();
            var modulemdl = new ModuleViewModel()
            {
                Name = moduleobj.Name,
                Technology = moduleobj.Technology,
                Proficiency = moduleobj.Proficiency
            };
            ViewData["moduleobj"] = moduleobj;
            return View("UpdateModule", modulemdl);
        }

        [Route("module/update/{Id}", Name = "updatemodule")]
        [HttpPost]
        public async Task<IActionResult> UpdateModule(int Id, ModuleViewModel moduleViewModel)
        {
            var moduleobj = await _db.Modules.Where(sk => sk.Id == Id).Include(sk => sk.CreatedBy).SingleOrDefaultAsync();
            if (ModelState.IsValid)
            {
                {
                    moduleobj.Name = moduleViewModel.Name;
                    moduleobj.Technology = moduleViewModel.Technology;
                    moduleobj.Proficiency= moduleViewModel.Proficiency;
                    _db.Modules.Update(moduleobj);
                    await _db.SaveChangesAsync();
                    TempData["Message"] = $"Module \"{moduleobj.Name}\" Updated Successfully !!";
                    TempData["Type"] = "success";
                    return RedirectToRoute("updatemodule");
                }
            }

            ViewData["moduleobj"] = moduleobj;
            return View("UpdateModule");
        }


        [Route("module/delete/{Id}", Name = "deletemodule")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var moduleobj = _db.Modules.SingleOrDefault(s => s.Id == id);
            if (moduleobj != null)
            {
                _db.Modules.Remove(moduleobj);
                await _db.SaveChangesAsync();
                TempData["Message"] = $"Module Deleted Successfully !!";
                TempData["Type"] = "success";
                return RedirectToRoute("addmodule");
            }
            TempData["Message"] = $"Module not found!!";
            TempData["Type"] = "danger";
            return RedirectToRoute("addmodule");
        }

        [Route("skillmodulemapping", Name = "skillmodulemapping")]
        public async Task<IActionResult> SkillModuleMapping()
        {
            var skills = await _db.Skills.Include(sk => sk.CreatedBy).Include(sk => sk.RelatedModules).ToListAsync();
            ViewData["skills"] = skills;
            return View("SkillModuleMapping");
        }
        
        [Route("addmodules/{Id}", Name = "addmodules")]
        public async Task<IActionResult> AddModules(int Id)
        {
            var skillobj = await _db.Skills.Include(sk => sk.CreatedBy).Include(sk=>sk.RelatedModules).Where(sk => sk.Id == Id).FirstOrDefaultAsync();
            var lstmdoules = new List<SelectListItem>();
            foreach(var item in await _db.Modules.ToListAsync())
            {
                lstmdoules.Add(new SelectListItem(item.Name, item.Id.ToString(), skillobj.RelatedModules.Select(x => x.Id).Contains(item.Id)));
            }
            ViewBag.lstmdoules = lstmdoules;
            ViewData["skillobj"] = skillobj;
            return View("AddModule");
        }
        [Route("addmodules/{Id}", Name = "addmodules")]
        [HttpPost]
        public async Task<IActionResult> AddModules(int Id, SkillModuleMappingViewModel skillModuleMappingViewModel)
        {
            var skillobj = await _db.Skills.Include(sk => sk.CreatedBy).Include(sk => sk.RelatedModules).Where(sk => sk.Id == Id).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                var lstmdoules = new List<Module>();

                foreach(var id in skillModuleMappingViewModel.ModuleNames)
                {
                    lstmdoules.Add(_db.Modules.Find(Convert.ToInt32(id)));
                }
                skillobj.RelatedModules = lstmdoules;
                _db.Skills.Update(skillobj);
                _db.SaveChanges();
                TempData["Message"] = $" Selected Modules added to {skillobj.Name} successfully!";
                TempData["Type"] = "success";
            }
            return RedirectToRoute("addmodules");
        }

        [Route("{skillId}/removemodule/{Id}", Name = "removemodule")]
        public async Task<IActionResult> RemoveModule(int Id, int skillId)
        {
            var skillobj = await _db.Skills.Include(sk => sk.CreatedBy).Include(sk => sk.RelatedModules).Where(sk => sk.Id == skillId).FirstOrDefaultAsync();
            var moduleobj =  await _db.Modules.Include(md => md.CreatedBy).Include(md => md.RelatedSkills).Where(md => md.Id == Id).FirstOrDefaultAsync();
            moduleobj.RelatedSkills.Remove(skillobj);
            await _db.SaveChangesAsync();
            return RedirectToRoute("addmodules", new {Id = skillobj.Id });
        }
    }
}
