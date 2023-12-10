using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AcademyPortalAPI.Models;
using Microsoft.AspNetCore.Identity;
using AcademyPortalAPI.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace AcademyPortalAPI.Repository.Batches
{
    public class BatchRepository : IBatchRepository
    {
        private readonly AcademyPortalDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public BatchRepository(AcademyPortalDbContext db, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task AddBatchAsync(ApplicationUser user, BatchDto batchDto)
        {

            var batch = _mapper.Map<Batch>(batchDto);
            batch.RelatedSkill = await _db.Skills.FindAsync(batchDto.RelatedSkillId);
            batch.RelatedModule = await _db.Modules.FindAsync(batchDto.RelatedModuleId);
            batch.CreatedBy = user;

            await _db.Batches.AddAsync(batch);
        }

        public async Task<Batch> GetBatchByIdAsync(int id)
        {
            return  await _db.Batches.FindAsync(id); 
        }

        public async Task<Batch> GetBatchByIdWithUsersAsync(int id)
        {
            var batch = await _db.Batches.Include(b => b.Users.OrderBy(x => x.UserId)).FirstOrDefaultAsync(b => b.Id == id);
            return batch;
        }

        public async Task<BatchDto> GetBatchDtoByIdWithUsersAsync(int id)
        {
            var batch =  await _db.Batches.Include(b => b.Users.OrderBy(x => x.UserId)).ProjectTo<BatchDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(b => b.Id == id);
            return batch;
        }

        public async Task<IEnumerable<BatchDto>> GetBatchDtosAssignedToUserWithUserAsync(string userId)
        {
            var query = _db.Batches.Include(b => b.CreatedBy).Include(b => b.Users).Where(b => b.Users.Any(bu => bu.UserId == userId)).AsQueryable();
            var batches = await query.ProjectTo<BatchDto>(_mapper.ConfigurationProvider).ToListAsync();
            return batches;
        }

        public async Task<IEnumerable<BatchDto>> GetBatchDtosWithUserAsync()
        {
            return await _db.Batches.Include(b => b.CreatedBy).ProjectTo<BatchDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public  async Task<DetailedBatchDto> GetDetailedBatchDtoAssignedToUserByIdAsync(int id, string userId)
        {
            var query = _db.Batches.Include(b => b.CreatedBy).Include(b => b.RelatedSkill).Include(b => b.RelatedModule).Include(b => b.Users).ThenInclude(bu => bu.User).Include(b => b.Users).ThenInclude(bu => bu.status).Where(b => b.Users.Any(bu => bu.UserId == userId)).AsQueryable();

            var bactch = await query.ProjectTo<DetailedBatchDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(b => b.Id == id);
            return bactch;
        }

        public async Task<DetailedBatchDto> GetDetailedBatchDtoByIdAsync(int id)
        {
            var query = _db.Batches.Include(b => b.CreatedBy).Include(b => b.RelatedSkill).Include(b => b.RelatedModule).Include(b => b.Users).ThenInclude(bu => bu.User).Include(b => b.Users).ThenInclude(bu => bu.status).AsQueryable();

            var bactch = await query.ProjectTo<DetailedBatchDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(b => b.Id == id);
            return bactch;
        }

        public async Task<IEnumerable<DetailedBatchDto>> GetDetailedBatchDtosAsync()
        {
            var query = _db.Batches.Include(b => b.CreatedBy).Include(b => b.RelatedSkill).Include(b => b.RelatedModule).Include(b => b.Users).ThenInclude(bu => bu.User).Include(b => b.Users).ThenInclude(bu => bu.status).AsQueryable();

            var bacthes = await query.ProjectTo<DetailedBatchDto>(_mapper.ConfigurationProvider).ToListAsync();
            return bacthes;
        }

        public async Task<bool> HasFacultyWithId(int batchId, string facultyId)
        {
            var batch = _db.Batches.Include(b => b.Users).Where(b => b.Id == batchId).AsQueryable();
            var isPresent = await batch.AnyAsync(b => b.Users.Any(bu => bu.UserId == facultyId));
            return isPresent;
        }

        public void RemoveFacultyFromBatch(Batch batch, BatchUser batchUser)
        {
            batch.Users.Remove(batchUser);
        }

        public async Task UpdateBatch(Batch batch, BatchDto batchDto)
        {
            batch.Technology = batchDto.Technology;
            batch.Batch_Start_Date = batchDto.Batch_Start_Date;
            batch.Batch_End_Date = batchDto.Batch_End_Date;
            batch.Batch_Capacity = batchDto.Batch_Capacity;
            batch.Classroom_Name = batchDto.Classroom_Name;
            batch.RelatedSkill = await _db.Skills.FindAsync(batchDto.RelatedSkillId);
            batch.RelatedModule = await _db.Modules.FindAsync(batchDto.RelatedModuleId);
            _db.Batches.Update(batch);
        }

        public void UpdateBatchUser(Batch batch)
        {
            _db.Batches.Update(batch);
        }
    }
}