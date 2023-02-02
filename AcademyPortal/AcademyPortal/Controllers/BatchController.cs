using AcademyPortal.Model;
using AcademyPortal.Models;
using AcademyPortal.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortal.Controllers
{

    public class BatchController : Controller
    {
        private readonly AcademyPortalDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public BatchController(AcademyPortalDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [Route("batch", Name = "addbatch")]
        public async Task<IActionResult> AddBatch()
        {
            var lstmodule = new List<SelectListItem>();
            foreach (var item in await _db.Modules.ToListAsync())
            {
                lstmodule.Add(new SelectListItem(item.Name, item.Id.ToString()));
            }
            ViewBag.lstmodule = lstmodule;

            var lstskill = new List<SelectListItem>();
            foreach (var item in await _db.Skills.ToListAsync())
            {
                lstskill.Add(new SelectListItem(item.Name, item.Id.ToString()));
            }
            ViewBag.lstskill = lstskill;

            //get all batch
            ViewData["Batches"] = await _db.Batches.Include(b => b.CreatedBy).ToListAsync();
            return View("AddBatch");
        }

        [Authorize(Roles = "Admin")]
        [Route("batch", Name = "addbatch")]
        [HttpPost]
        public async Task<IActionResult> AddBatch(AddBatchViewModel addBatchViewModel)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            if (ModelState.IsValid)
            {
                var batchobj = new Batch()
                {
                    RelaedSkill = await _db.Skills.FindAsync(Convert.ToInt32(addBatchViewModel.RelaedSkill)),
                    RelaedModule = await _db.Modules.FindAsync(Convert.ToInt32(addBatchViewModel.RelaedModule)),
                    Technology = addBatchViewModel.Technology,
                    Batch_Start_Date = addBatchViewModel.Batch_Start_Date,
                    Batch_End_Date = addBatchViewModel.Batch_End_Date,
                    Batch_Capacity = addBatchViewModel.Batch_Capacity,
                    Classroom_Name = addBatchViewModel.Classroom_Name,
                    CreatedBy = user
                };
                await _db.Batches.AddAsync(batchobj);
                await _db.SaveChangesAsync();
                TempData["Message"] = $" Batch Added to Databse Successfully!";
                TempData["Type"] = "success";
                return RedirectToRoute("addbatch");
            }
            var lstmodule = new List<SelectListItem>();
            foreach (var item in await _db.Modules.ToListAsync())
            {
                lstmodule.Add(new SelectListItem(item.Name, item.Id.ToString()));
            }
            ViewBag.lstmodule = lstmodule;

            var lstskill = new List<SelectListItem>();
            foreach (var item in await _db.Skills.ToListAsync())
            {
                lstskill.Add(new SelectListItem(item.Name, item.Id.ToString()));
            }
            ViewBag.lstskill = lstskill;
            ViewData["Batches"] = await _db.Batches.Include(b => b.CreatedBy).ToListAsync();
            return View("AddBatch");
        }

        [Authorize(Roles = "Admin")]
        [Route("batch/{Id}", Name = "updatebatch")]
        public async Task<IActionResult> UpdateBatch(int Id)

        {
            var batchobj = _db.Batches.Include(b => b.CreatedBy).Include(b => b.RelaedSkill).Include(b => b.RelaedModule).Include(b => b.Users).ThenInclude(bu => bu.User).Include(b => b.Users).ThenInclude(bu => bu.status).FirstOrDefault(x => x.Id == Id);

            var lstmodule = new List<SelectListItem>();
            foreach (var item in await _db.Modules.ToListAsync())
            {
                lstmodule.Add(new SelectListItem(item.Name, item.Id.ToString(), batchobj.RelaedModule.Id == item.Id));
            }
            ViewBag.lstmodule = lstmodule;

            var lstskill = new List<SelectListItem>();
            foreach (var item in await _db.Skills.ToListAsync())
            {
                lstskill.Add(new SelectListItem(item.Name, item.Id.ToString(), batchobj.RelaedSkill.Id == item.Id));
            }
            ViewBag.lstskill = lstskill;

            //get batch by Id
            var batchvm = new AddBatchViewModel()
            {
                Technology = batchobj.Technology,
                Batch_Start_Date = batchobj.Batch_Start_Date,
                Batch_End_Date = batchobj.Batch_End_Date,
                Batch_Capacity = batchobj.Batch_Capacity,
                Classroom_Name = batchobj.Classroom_Name
            };
            ViewData["batchobj"] = batchobj;
            return View("UpdateBatch", batchvm);
        }

        [Authorize(Roles = "Admin")]
        [Route("batch/{Id}", Name = "updatebatch")]
        [HttpPost]
        public async Task<IActionResult> UpdateBatch(int Id, AddBatchViewModel addBatchViewModel)
        {
            if (ModelState.IsValid)
            {
                var batchobj = _db.Batches.Include(b => b.CreatedBy).Include(b => b.RelaedSkill).Include(b => b.RelaedModule).FirstOrDefault(x => x.Id == Id);
                batchobj.RelaedSkill = await _db.Skills.FindAsync(Convert.ToInt32(addBatchViewModel.RelaedSkill));
                batchobj.RelaedModule = await _db.Modules.FindAsync(Convert.ToInt32(addBatchViewModel.RelaedModule));
                batchobj.Technology = addBatchViewModel.Technology;
                batchobj.Batch_Start_Date = addBatchViewModel.Batch_Start_Date;
                batchobj.Batch_End_Date = addBatchViewModel.Batch_End_Date;
                batchobj.Batch_Capacity = addBatchViewModel.Batch_Capacity;
                batchobj.Classroom_Name = addBatchViewModel.Classroom_Name;

                await _db.SaveChangesAsync();
            }
            return RedirectToRoute("updatebatch");
        }

        [Authorize(Roles = "Admin")]
        [Route("addfaculty/{Id}", Name = "addfaculty")]
        public async Task<IActionResult> AddFaculty(int Id)
        {
            var batchobj = _db.Batches.Include(b => b.CreatedBy).Include(b => b.RelaedSkill).Include(b => b.RelaedModule).Include(b => b.Users).ThenInclude(bu => bu.User).ThenInclude(bu => bu.status).FirstOrDefault(x => x.Id == Id);

            var lstfaculty = new List<SelectListItem>();
            foreach (var item in await _userManager.GetUsersInRoleAsync("Faculty"))
            {
                lstfaculty.Add(new SelectListItem(item.Email, item.Id, batchobj.Users.Select(u => u.UserId).Contains(item.Id)));
            }
            ViewBag.lstfaculty = lstfaculty;

            ViewData["batchobj"] = batchobj;
            return View("AddFaculty");
        }

        [Authorize(Roles = "Admin")]
        [Route("addfaculty/{Id}", Name = "addfaculty")]
        [HttpPost]
        public async Task<IActionResult> AddFaculty(int Id, AddFacultyViewModel addFacultyViewModel)
        {
            if (ModelState.IsValid)
            {
                var batchobj = _db.Batches.Include(b => b.Users.OrderBy(x => x.UserId)).Single(b => b.Id == Id);
                var lstbatchuser = new List<BatchUser>();
                foreach (var id in addFacultyViewModel.Faculties)
                {
                    if (!batchobj.Users.Select(bu => bu.UserId).Contains(id))
                    {
                        var user = _userManager.Users.Single(u => u.Id == id);
                        var newbatchobj = new BatchUser()
                        {
                            Batch = batchobj,
                            User = user,
                            status = await _db.AllStatus.FindAsync(1)
                        };
                        batchobj.Users.Add(newbatchobj);
                    }
                }
                await _db.SaveChangesAsync();
            }
            return RedirectToRoute("addfaculty");
        }

        [Authorize(Roles = "Admin")]
        [Route("{batchId}/removefaculty/{Id}", Name = "removefaculty")]
        public async Task<IActionResult> RemoveFaculty(int batchId, string Id)
        {
            var batchobj = _db.Batches.Include(b => b.Users.OrderBy(x => x.UserId)).Single(b => b.Id == batchId);
            var user = batchobj.Users.SingleOrDefault(bu => bu.UserId == Id);
            if (user != null)
            {
                batchobj.Users.Remove(user);
                await _db.SaveChangesAsync();
            }
            return RedirectToRoute("addfaculty", new { Id = batchobj.Id });
        }

        [Authorize(Roles = "Faculty")]
        [Route("mybatches", Name = "mybatches")]
        public async Task<IActionResult> MyBatch()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var myuser = _userManager.Users.Include(u => u.Batches).ThenInclude(b => b.Batch).ThenInclude(b => b.CreatedBy).SingleOrDefault(u => u.Id == user.Id);

            var batches = new List<Batch>();

            foreach (var batch in user.Batches.ToList())
            {
                batches.Add(batch.Batch);
            }
            ViewData["Batches"] = batches;
            return View("UserBatches");
        }

        [Authorize(Roles = "Faculty")]
        [Route("update-batch-status/{Id}", Name = "updatebatchstatus")]
        public async Task<IActionResult> UpdateBatchStatus(int Id)
        {
            var batchobj = _db.Batches.Include(b => b.CreatedBy).Include(b => b.RelaedSkill).Include(b => b.RelaedModule).Include(b => b.Users).ThenInclude(bu => bu.User).FirstOrDefault(x => x.Id == Id);
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var lststatus = new List<SelectListItem>();
            foreach (var item in await _db.AllStatus.ToListAsync())
            {
                lststatus.Add(new SelectListItem(item.Name, item.Id.ToString(), batchobj.Users.Where(bu => bu.BatchId == Id && bu.UserId == user.Id).Select(bu => bu.status.Id).Contains(item.Id)));
            }
            ViewBag.lststatus = lststatus;
            ViewData["batchobj"] = batchobj;

            return View("UpdateBatchStatus");
        }

        [Authorize(Roles = "Faculty")]
        [Route("update-batch-status/{Id}", Name = "updatebatchstatus")]
        [HttpPost]
        public async Task<IActionResult> UpdateBatchStatus(int Id, UpdateBatchStatusViewModel  updateBatchStatusViewModel)
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var batchobj = _db.Batches.Include(b => b.CreatedBy).Include(b => b.RelaedSkill).Include(b => b.RelaedModule).Include(b => b.Users).ThenInclude(bu => bu.User).Include(b => b.Users).ThenInclude(bu => bu.status).FirstOrDefault(x => x.Id == Id);
            if (ModelState.IsValid)
            {
                if (batchobj != null)
                {
                    var buobj = batchobj.Users.Where(bu => bu.BatchId == Id && bu.UserId == user.Id).SingleOrDefault();
                    buobj.status = _db.AllStatus.Find(Convert.ToInt32(updateBatchStatusViewModel.Status));
                    await _db.SaveChangesAsync();
                }
            }
            return RedirectToRoute("updatebatchstatus", new { Id = batchobj.Id });
        }
    }
}