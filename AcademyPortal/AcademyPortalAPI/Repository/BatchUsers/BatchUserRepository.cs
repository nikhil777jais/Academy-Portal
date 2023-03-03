using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AcademyPortalAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace AcademyPortalAPI.Repository.BatchUsers
{
    public class BatchUserRepository : IBatchUserRepository
    {
        private readonly AcademyPortalDbContext _db;
        private readonly IMapper _mapper;

        public BatchUserRepository(AcademyPortalDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Batch>> GetBatchesAssignedToUserByUserId(string userId)
        {
            var batchUsers = await _db.BatchUser.Where(bu => bu.UserId == userId).Include(bu => bu.Batch).ThenInclude(b => b.CreatedBy).ToListAsync();
            return batchUsers.Select(bu => bu.Batch).ToList();;
        }

        public async Task<BatchUser> GetBatchUserById(int batchId, string userId)
        {
            return await _db.BatchUser.FirstOrDefaultAsync(bu => bu.UserId == userId && bu.BatchId == batchId);
        }

        public void RemoveBatchUser(BatchUser batchUser)
        {
            _db.BatchUser.Remove(batchUser);
        }
    }
}