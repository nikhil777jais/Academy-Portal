using AcademyPortal.Model;
using AcademyPortal.Models;
using AcademyPortal.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortal.Controllers
{ 
    [Authorize(Roles = "Admin")]
    public class SkillController : Controller
    {
        private readonly AcademyPortalDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public SkillController(AcademyPortalDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Route("skill", Name = "addskill")]
        public async Task<IActionResult> AddAndListSkill()
        {
            var skill = await _db.Skills.Include(s => s.CreatedBy).ToListAsync();
            ViewData["skills"] = await _db.Skills.Include(s => s.CreatedBy).ToListAsync();
            return View("AddAndListSkill");
        }
        [Route("skill", Name = "addskill")]
        [HttpPost]
        public async Task<IActionResult> AddAndListSkill(SkillViewModel skillViewModel)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (ModelState.IsValid)
            {
                var skillobj = new Skill()
                {
                    Name = skillViewModel.Name,
                    Family = skillViewModel.Family,
                    CreatedBy = user
                };
                await _db.Skills.AddAsync(skillobj);
                await _db.SaveChangesAsync();
                TempData["Message"] = $"Skill Created Successfully !!";
                TempData["Type"] = "success";
                return RedirectToRoute("addskill");
            }
            ViewData["skills"] = await _db.Skills.Include(s => s.CreatedBy).ToListAsync();
            return View("AddAndListSkill");
        }

        [Route("skill/update/{Id}", Name = "updateskill")]
        public async Task<IActionResult> UpdateSkill(int Id)
        {
            var skillobj = await _db.Skills.Where(sk => sk.Id == Id).Include(sk => sk.CreatedBy).SingleOrDefaultAsync();
            var skmdl = new SkillViewModel()
            {
                Name = skillobj.Name,
                Family = skillobj.Family
            };
            ViewData["skillobj"] = skillobj;
            return View("UpdateSkill", skmdl);
        }

        [Route("skill/update/{Id}", Name = "updateskill")]
        [HttpPost]
        public async Task<IActionResult> UpdateSkill(int Id, SkillViewModel skillViewModel)
        {
            var skillobj = await _db.Skills.Where(sk => sk.Id == Id).Include(sk => sk.CreatedBy).SingleOrDefaultAsync();
            if (ModelState.IsValid)
            {
                {
                    skillobj.Name = skillViewModel.Name;
                    skillobj.Family = skillViewModel.Family;
                    _db.Skills.Update(skillobj);
                    await _db.SaveChangesAsync();
                    TempData["Message"] = $"Skill \"{skillobj.Name}\" Updated Successfully !!";
                    TempData["Type"] = "success";
                    return RedirectToRoute("updateskill");
                }
            }
            
            ViewData["skillobj"] = skillobj;
            return View("UpdateSkill");
        }


        [Route("skill/delete/{Id}", Name = "deleteskill")]
        public async Task<IActionResult> DeleteSkill(int Id)
        {
            var skill = await _db.Skills.FindAsync(Id);
            if (skill != null)
            {
                _db.Skills.Remove(skill);
                await _db.SaveChangesAsync();
                TempData["Message"] = $"Skill \"{skill.Name}\" Deleted Successfully !!";
                TempData["Type"] = "success";
                return RedirectToRoute("addskill");
            }
            return View("AddAndListSkill");
        }
    }
}
