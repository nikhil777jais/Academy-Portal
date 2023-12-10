using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AcademyPortalAPI.Models;

namespace AcademyPortalAPI.Repository.BatchUsers
{
    public interface IBatchUserRepository
    {
        Task<IEnumerable<Batch>> GetBatchesAssignedToUserByUserId(string userId);
        Task<BatchUser> GetBatchUserById(int batchId, string userId);
        void RemoveBatchUser(BatchUser batchUser);
    }
}