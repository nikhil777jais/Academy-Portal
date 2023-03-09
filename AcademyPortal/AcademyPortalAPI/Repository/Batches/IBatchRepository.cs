using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;

namespace AcademyPortalAPI.Repository.Batches
{
    public interface IBatchRepository
    {
        Task<Batch> GetBatchByIdWithUsersAsync(int id);

        Task<BatchDto> GetBatchDtoByIdWithUsersAsync(int id);
        Task<IEnumerable<BatchDto>> GetBatchDtosWithUserAsync();
        Task<IEnumerable<BatchDto>> GetBatchDtosAssignedToUserWithUserAsync(string userId);

        Task<DetailedBatchDto> GetDetailedBatchDtoByIdAsync(int id);
        Task<DetailedBatchDto> GetDetailedBatchDtoAssignedToUserByIdAsync(int id, string userId);
        Task<IEnumerable<DetailedBatchDto>> GetDetailedBatchDtosAsync();

        Task<Batch> GetBatchByIdAsync(int id);
        Task AddBatchAsync(ApplicationUser user, BatchDto batchDto);
        Task UpdateBatch(Batch batch,BatchDto batchDto);
        void UpdateBatchUser(Batch batch);
        Task<bool> HasFacultyWithId(int batchId, string facultyId);
        void RemoveFacultyFromBatch(Batch batch, BatchUser batchUser);
    }
}