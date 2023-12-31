using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AcademyPortal.Models;
using Microsoft.AspNetCore.Identity;
using AcademyPortal.DTOs;

namespace AcademyPortal.Repository.Batches
{
    public class BatchRepository : IBatchRepository
    {
        private readonly AcademyPortalDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public BatchRepository(AcademyPortalDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task AddBatchAsync(ApplicationUser user, AddBatchDto addBatchDto)
        {
            var batch = new Batch()
            {
                RelaedSkill = await _db.Skills.FindAsync(Convert.ToInt32(addBatchDto.RelaedSkill)),
                RelaedModule = await _db.Modules.FindAsync(Convert.ToInt32(addBatchDto.RelaedModule)),
                Technology = addBatchDto.Technology,
                Batch_Start_Date = addBatchDto.Batch_Start_Date,
                Batch_End_Date = addBatchDto.Batch_End_Date,
                Batch_Capacity = addBatchDto.Batch_Capacity,
                Classroom_Name = addBatchDto.Classroom_Name,
                CreatedBy = user
            };
            await _db.Batches.AddAsync(batch);
        }

        public async Task<Batch> GetBatchByIdAsync(int id)
        {
            return await _db.Batches.FindAsync(id);
        }

        public async Task<Batch> GetBatchByIdWithUsersAsync(int id)
        {
            var batch =  await _db.Batches.Include(b => b.Users.OrderBy(x => x.UserId))
            .FirstOrDefaultAsync(b => b.Id == id);
            return batch;
        }

        public async Task<IEnumerable<Batch>> GetBatchesAsync()
        {
            return await _db.Batches.ToListAsync();
        }

        public async Task<IEnumerable<Batch>> GetBatchesWithUserAsync()
        {
            return await _db.Batches.Include(b => b.CreatedBy).ToListAsync();
        }

        public async Task<Batch> GetDetailedBatchByIdAsync(int id)
        {
            var batch = await _db.Batches.Include(b => b.CreatedBy).Include(b => b.RelaedSkill).Include(b => b.RelaedModule).Include(b => b.Users).ThenInclude(bu => bu.User).Include(b => b.Users).ThenInclude(bu => bu.status).FirstOrDefaultAsync(x => x.Id == id);
            return batch;
        }

        public void RemoveFacultyFromBatch(Batch batch, BatchUser batchUser)
        {
            batch.Users.Remove(batchUser);
        }

        public void UpdateBatch(Batch batch)
        {
            _db.Batches.Update(batch);
        }
    }
}