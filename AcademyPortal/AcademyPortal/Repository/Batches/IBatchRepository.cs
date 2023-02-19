using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Models;
using AcademyPortal.ViewModel;

namespace AcademyPortal.Repository.Batches
{
    public interface IBatchRepository
    {
        Task<IEnumerable<Batch>> GetBatchesAsync();
        Task<IEnumerable<Batch>> GetBatchesWithUserAsync();
        Task<Batch> GetBatchByIdAsync(int id);
        Task<Batch> GetBatchByIdWithUsersAsync(int id);
        Task<Batch> GetDetailedBatchByIdAsync(int id);
        Task AddBatchAsync(ApplicationUser user, AddBatchViewModel addBatchViewModel);
        void UpdateBatch(Batch batch);
        void RemoveFacultyFromBatch(Batch batch, BatchUser batchUser);
    }
}