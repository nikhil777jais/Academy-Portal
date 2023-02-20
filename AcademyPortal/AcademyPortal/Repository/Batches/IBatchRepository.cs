using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.DTOs;
using AcademyPortal.Models;

namespace AcademyPortal.Repository.Batches
{
    public interface IBatchRepository
    {
        Task<IEnumerable<Batch>> GetBatchesAsync();
        Task<IEnumerable<Batch>> GetBatchesWithUserAsync();
        Task<Batch> GetBatchByIdAsync(int id);
        Task<Batch> GetBatchByIdWithUsersAsync(int id);
        Task<Batch> GetDetailedBatchByIdAsync(int id);
        Task AddBatchAsync(ApplicationUser user, AddBatchDto addBatchDto);
        void UpdateBatch(Batch batch);
        void RemoveFacultyFromBatch(Batch batch, BatchUser batchUser);
    }
}