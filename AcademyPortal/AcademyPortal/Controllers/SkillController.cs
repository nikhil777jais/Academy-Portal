using AcademyPortal.Models;
using AcademyPortal.Repository.UnitOfWork;
using AcademyPortal.ViewModel;
using AcademyPortal.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortal.Controllers
{ 
    [Authorize(Roles = "Admin")]
    public class SkillController : Controller
    {
        private readonly IUnitOfWork _uow;

        public SkillController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Route("skill", Name = "addSkill")]
        public async Task<IActionResult> AddAndListSkill()
        {

            var skills = await _uow.SkillRepository.GetSkillsAsync();
            ViewData["skills"] = skills;
            return View("AddAndListSkill");
        }
        [Route("skill", Name = "addSkill")]
        [HttpPost]
        public async Task<IActionResult> AddAndListSkill(SkillViewModel skillViewModel)
        {
            var user = await _uow.UserRepository.GetUserByClaimsAsync(User);
            if (ModelState.IsValid)
            {
                await _uow.SkillRepository.AddSkillAsync(user, skillViewModel);
                if(await _uow.SaveChangesAsync()){
                    TempData["Message"] = $"Skill Created Successfully !!";
                    TempData["Type"] = "success";
                }else{
                    TempData["Message"] = $"Error while saving changes";
                    TempData["Type"] = "danger";
                }
                return RedirectToRoute("addSkill");
            }
            return RedirectToRoute("addSkill");
            // ViewData["skills"] = await _db.Skills.Include(s => s.CreatedBy).ToListAsync();
            // return View("AddAndListSkill");
        }

        [Route("skill/update/{id}", Name = "updateSkill")]
        public async Task<IActionResult> UpdateSkill(int id)
        {
            var skill = await _uow.SkillRepository.GetSkillByIdAsync(id);
            var skillVm = new SkillViewModel()
            {
                Name = skill.Name,
                Family = skill.Family
            };
            ViewData["skill"] = skill;
            return View("UpdateSkill", skillVm);
        }

        [Route("skill/update/{id}", Name = "updateSkill")]
        [HttpPost]
        public async Task<IActionResult> UpdateSkill(int id, SkillViewModel skillViewModel)
        {
            var skill = await _uow.SkillRepository.GetSkillByIdAsync(id);
            if (ModelState.IsValid)
            {
                {
                    skill.Name = skillViewModel.Name;
                    skill.Family = skillViewModel.Family;
                    _uow.SkillRepository.UpdateSkill(skill);
                    if(await _uow.SaveChangesAsync()){
                        TempData["Message"] = $"Skill \"{skill.Name}\" Updated Successfully !!";
                        TempData["Type"] = "success";    
                    }
                    return RedirectToRoute("updateSkill");
                }
            }
            ViewData["skill"] = skill;
            return View("UpdateSkill");
        }


        [Route("skill/delete/{id}", Name = "deleteSkill")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await _uow.SkillRepository.GetSkillByIdAsync(id);
            if (skill != null)
            {
                _uow.SkillRepository.RemoveSkill(skill);
                if(await _uow.SaveChangesAsync()){
                    TempData["Message"] = $"Skill \"{skill.Name}\" Deleted Successfully !!";
                    TempData["Type"] = "success";
                }
                return RedirectToRoute("addSkill");
            }
            return View("AddAndListSkill");
        }
    }
}
