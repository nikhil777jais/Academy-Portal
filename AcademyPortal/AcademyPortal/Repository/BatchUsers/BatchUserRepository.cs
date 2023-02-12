using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AcademyPortal.Models;
using Microsoft.EntityFrameworkCore;
namespace AcademyPortal.Repository.BatchUsers
{
    public class BatchUserRepository : IBatchUserRepository
    {
        private readonly AcademyPortalDbContext _db;

        public BatchUserRepository(AcademyPortalDbContext db)
        {
            _db = db;
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