using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AcademyPortal.Models;

namespace AcademyPortal.Repository.BatchUsers
{
    public interface IBatchUserRepository
    {
        Task<IEnumerable<Batch>> GetBatchesAssignedToUserByUserId(string userId);
        Task<BatchUser> GetBatchUserById(int batchId, string userId);
        void RemoveBatchUser(BatchUser batchUser);
    }
}