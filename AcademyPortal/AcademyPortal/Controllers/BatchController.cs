using AcademyPortal.Models;
using AcademyPortal.Repository.UnitOfWork;
using AcademyPortal.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcademyPortal.DTOs;

namespace AcademyPortal.Controllers
{

    public class BatchController : Controller
    {
        private readonly IUnitOfWork _uow;

        public BatchController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Authorize(Roles = "Admin")]
        [Route("batch", Name = "addBatch")]
        public async Task<IActionResult> AddBatch()
        {
           
            ViewBag.listModule = GetModuleSelectList(await _uow.ModuleRepository.GetModulesAsync());
            ViewBag.listSkill = GetSkillSelectList(await _uow.SkillRepository.GetSkillsAsync());
            //get all batch
            ViewData["Batches"] = await _uow.BatchRepository.GetBatchesWithUserAsync();
            return View("AddBatch");
        }

        [Authorize(Roles = "Admin")]
        [Route("batch", Name = "addBatch")]
        [HttpPost]
        public async Task<IActionResult> AddBatch(AddBatchDto addBatchDto)
        {
            var user = await _uow.UserRepository.GetUserByClaimsAsync(User);
            if (ModelState.IsValid)
            {
                await _uow.BatchRepository.AddBatchAsync(user, addBatchDto);
                if(await _uow.SaveChangesAsync()){
                    TempData["Message"] = $" Batch Added to Database Successfully!";
                    TempData["Type"] = "success";
                }else{
                    TempData["Message"] = $"Error while saving batch";
                    TempData["Type"] = "danger";
                }
                return RedirectToRoute("addBatch");
            }
            // to throw model validation error
            ViewBag.listModule = GetModuleSelectList(await _uow.ModuleRepository.GetModulesAsync());
            ViewBag.listSkill = GetSkillSelectList(await _uow.SkillRepository.GetSkillsAsync());
            ViewData["Batches"] = await _uow.BatchRepository.GetBatchesWithUserAsync();
            return View("AddBatch");
        }

        [Authorize(Roles = "Admin")]
        [Route("batch/{id}", Name = "updateBatch")]
        public async Task<IActionResult> UpdateBatch(int id)
        {
            var batch = await _uow.BatchRepository.GetDetailedBatchByIdAsync(id);

            var listModule = new List<SelectListItem>();
            foreach (var item in await _uow.ModuleRepository.GetModulesAsync())
            {
                listModule.Add(new SelectListItem(item.Name, item.Id.ToString(), batch.RelaedModule.Id == item.Id));
            }
            ViewBag.listModule = listModule;

            var listSkill = new List<SelectListItem>();
            foreach (var item in await _uow.SkillRepository.GetSkillsAsync())
            {
                listSkill.Add(new SelectListItem(item.Name, item.Id.ToString(), batch.RelaedSkill.Id == item.Id));
            }
            ViewBag.listSkill = listSkill;

            //get batch by Id
            var batchvm = new AddBatchDto()
            {
                Technology = batch.Technology,
                Batch_Start_Date = batch.Batch_Start_Date,
                Batch_End_Date = batch.Batch_End_Date,
                Batch_Capacity = batch.Batch_Capacity,
                Classroom_Name = batch.Classroom_Name
            };
            ViewData["batch"] = batch;
            return View("UpdateBatch", batchvm);
        }

        [Authorize(Roles = "Admin")]
        [Route("batch/{id}", Name = "updateBatch")]
        [HttpPost]
        public async Task<IActionResult> UpdateBatch(int id, AddBatchDto addBatchDto)
        {
            Console.WriteLine("*********************************************");
            Console.WriteLine(id);
            Console.WriteLine("*********************************************");
            if (ModelState.IsValid)
            {
                Console.WriteLine("valid");
                var batch = await _uow.BatchRepository.GetBatchByIdAsync(id);
                // var batch = _db.Batches.Include(b => b.CreatedBy).Include(b => b.RelaedSkill).Include(b => b.RelaedModule).FirstOrDefault(x => x.Id == id);
                batch.RelaedSkill = await _uow.SkillRepository.GetSkillByIdAsync(Convert.ToInt32(addBatchDto.RelaedSkill));
                batch.RelaedModule = await _uow.ModuleRepository.GetModuleByIdAsync(Convert.ToInt32(addBatchDto.RelaedModule));
                batch.Technology = addBatchDto.Technology;
                batch.Batch_Start_Date = addBatchDto.Batch_Start_Date;
                batch.Batch_End_Date = addBatchDto.Batch_End_Date;
                batch.Batch_Capacity = addBatchDto.Batch_Capacity;
                batch.Classroom_Name = addBatchDto.Classroom_Name;
                _uow.BatchRepository.UpdateBatch(batch);
                if(await _uow.SaveChangesAsync()){
                    TempData["Message"] = $" Batch Updated Successfully!";
                    TempData["Type"] = "success";
                }else{
                    TempData["Message"] = $"Error while updating batch";
                    TempData["Type"] = "danger";
                }
            }
            return RedirectToRoute("updateBatch");
        }

        [Authorize(Roles = "Admin")]
        [Route("addFaculty/{id}", Name = "addFaculty")]
        public async Task<IActionResult> AddFaculty(int id)
        {
            var batch = await _uow.BatchRepository.GetDetailedBatchByIdAsync(id);

            var listFaculty = new List<SelectListItem>();
            foreach (var item in await _uow.UserRepository.GetUsersInRoleAsync("Faculty"))
            {
                listFaculty.Add(new SelectListItem(item.Email, item.Id, batch.Users.Select(u => u.UserId).Contains(item.Id)));
            }
            ViewBag.listFaculty = listFaculty;

            ViewData["batch"] = batch;
            return View("AddFaculty");
        }

        [Authorize(Roles = "Admin")]
        [Route("addFaculty/{id}", Name = "addFaculty")]
        [HttpPost]
        public async Task<IActionResult> AddFaculty(int id, AddFacultyDto addFacultyDto)
        {
            if (ModelState.IsValid)
            {
                var batch = await _uow.BatchRepository.GetBatchByIdWithUsersAsync(id);
                var listBatchUser = new List<BatchUser>();
                foreach (var Id in addFacultyDto.Faculties)
                {
                    if (!batch.Users.Select(bu => bu.UserId).Contains(Id))
                    {
                        var user = await _uow.UserRepository.GetUserByIdAsync(Id);
                        var newBatch = new BatchUser()
                        {
                            Batch = batch,
                            User = user,
                            status = await _uow.StatusRepository.GetStatusByIdAsync(1)
                        };
                        batch.Users.Add(newBatch);
                    }
                }
                _uow.BatchRepository.UpdateBatch(batch);
                if(await _uow.SaveChangesAsync()){
                    TempData["Message"] = $"Faculties Added Successfully!";
                    TempData["Type"] = "success";
                }else{
                    TempData["Message"] = $"Error while adding faculties";
                    TempData["Type"] = "danger";
                }
            }
            return RedirectToRoute("addFaculty");
        }

        [Authorize(Roles = "Admin")]
        [Route("{batchId}/removeFaculty/{id}", Name = "removeFaculty")]
        public async Task<IActionResult> RemoveFaculty(int batchId, string id)
        {
            var batchUser = await _uow.BatchUserRepository.GetBatchUserById(batchId, id);
            if (batchUser != null)
            {
                _uow.BatchUserRepository.RemoveBatchUser(batchUser);
                if(await _uow.SaveChangesAsync()){
                    TempData["Message"] = $"Faculty Removed Successfully!";
                    TempData["Type"] = "success";
                }else{
                    TempData["Message"] = $"Error while removing Faculty";
                    TempData["Type"] = "danger";
                }
            }
            return RedirectToRoute("addFaculty", new { Id = batchId });
        }

        [Authorize(Roles = "Faculty")]
        [Route("myBatches", Name = "myBatches")]
        public async Task<IActionResult> MyBatch()
        {
            var batches = await _uow.BatchUserRepository.GetBatchesAssignedToUserByUserId(User.GetUserId());
            ViewData["Batches"] = batches;
            return View("UserBatches");
        }

        [Authorize(Roles = "Faculty")]
        [Route("update-batch-status/{id}", Name = "updateBatchStatus")]
        public async Task<IActionResult> UpdateBatchStatus(int id)
        {
            var batch = await _uow.BatchRepository.GetDetailedBatchByIdAsync(id);
            var user = await _uow.UserRepository.GetUserByClaimsAsync(User);
            
            var listStatus = new List<SelectListItem>();
            foreach (var item in await _uow.StatusRepository.GetAllStatus())
            {
                listStatus.Add(new SelectListItem(item.Name, item.Id.ToString(), batch.Users.Where(bu => bu.BatchId == id && bu.UserId == user.Id).Select(bu => bu.status.Id).Contains(item.Id)));
            }
            ViewBag.listStatus = listStatus;
            ViewData["batch"] = batch;
            return View("UpdateBatchStatus");
        }

        [Authorize(Roles = "Faculty")]
        [Route("update-batch-status/{id}", Name = "updateBatchStatus")]
        [HttpPost]
        public async Task<IActionResult> UpdateBatchStatus(int id, UpdateBatchStatusDto  updateBatchStatusDto)
        {
            var batch = await _uow.BatchRepository.GetDetailedBatchByIdAsync(id);
            var user = await _uow.UserRepository.GetUserByClaimsAsync(User);
            if (ModelState.IsValid)
            {
                if (batch != null)
                {
                    var batchUser = batch.Users.Where(bu => bu.BatchId == id && bu.UserId == user.Id).SingleOrDefault();
                    batchUser.status = await _uow.StatusRepository.GetStatusByIdAsync(Convert.ToInt32(updateBatchStatusDto.Status));
                    if(await _uow.SaveChangesAsync()){
                        TempData["Message"] = $"Batch status updated Successfully!";
                        TempData["Type"] = "success";
                    }else{
                        TempData["Message"] = $"Error while batch status";
                        TempData["Type"] = "danger";
                    }
                }
            }
            return RedirectToRoute("updateBatchStatus", new { Id = batch.Id });
        }

        private List<SelectListItem> GetModuleSelectList(IEnumerable<Module> modules){
            var listModule = new List<SelectListItem>();
            foreach (var item in modules)
            {
                listModule.Add(new SelectListItem(item.Name, item.Id.ToString()));
            }
            return listModule;
        }
        private List<SelectListItem> GetSkillSelectList(IEnumerable<Skill> skills){
            var listSkill = new List<SelectListItem>();
            foreach (var item in skills)
            {
                listSkill.Add(new SelectListItem(item.Name, item.Id.ToString()));
            }
            return listSkill;
        }
    }
}