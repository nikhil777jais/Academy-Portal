using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;
using AcademyPortalAPI.Extensions;
using AcademyPortalAPI.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademyPortalAPI.Controllers
{

    public class BatchController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;

        public BatchController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getBatches")]
        public async Task<IActionResult> GetBatches()
        {
            var batches = await _uow.BatchRepository.GetBatchDtosWithUserAsync();
            return Ok(batches);
        }

        [Authorize(Roles = "Faculty")]
        [HttpGet("getUserBatches")]
        public async Task<IActionResult> GetBatchesAssignedToUser()
        {
            var user = await _uow.UserRepository.GetUserByClaimsAsync(User);

            var batches = await _uow.BatchRepository.GetBatchDtosAssignedToUserWithUserAsync(user.Id);
            return Ok(batches);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getBatch/{id}")]
        public async Task<IActionResult> GetBatchById(int id)
        {
            var batches = await _uow.BatchRepository.GetBatchDtoByIdWithUsersAsync(id);
            return Ok(batches);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getDetailedBatches")]
        public async Task<IActionResult> GetDetailedBatches()
        {
            var batches = await _uow.BatchRepository.GetDetailedBatchDtosAsync();
            return Ok(batches);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("getDetailedBatch/{id}")]
        public async Task<IActionResult> GetDetailedBatchById(int id)
        {
            var batches = await _uow.BatchRepository.GetDetailedBatchDtoByIdAsync(id);
            return Ok(batches);
        }

        [Authorize(Roles = "Faculty")]
        [HttpGet("getDetailedUserBatch/{id}")]
        public async Task<IActionResult> GetDetailedBatchAssignedToUserById(int id)
        {
            var user = await _uow.UserRepository.GetUserByClaimsAsync(User);
            var batch = await _uow.BatchRepository.GetDetailedBatchDtoAssignedToUserByIdAsync(id, user.Id);
            if (batch == null)
                return BadRequest(new { errors = $"User Id -> {user.Id} is not associated with batch Id -> {id}" });
            return Ok(batch);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addBatch")]
        public async Task<IActionResult> AddBatch(BatchDto batchDto)
        {
            var user = await _uow.UserRepository.GetUserByClaimsAsync(User);

            await _uow.BatchRepository.AddBatchAsync(user, batchDto);
            if (!await _uow.SaveChangesAsync()) return BadRequest(new { errors = $"Unable to add batch" });

            return Ok(new { message = $"batch added" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("updateBatch/{id}")]
        public async Task<IActionResult> UpdateBatch(BatchDto batchDto, int id)
        {

            var batch = await _uow.BatchRepository.GetBatchByIdAsync(id);

            if (batch == null) return BadRequest(new { error = $"batch not found with id {id}" });

            await _uow.BatchRepository.UpdateBatch(batch, batchDto);

            if (!await _uow.SaveChangesAsync()) return BadRequest(new { error = $"Unable to update batch" });
            return Ok(new { message = $"batch with id -> {id} updated" });
        }
        /*
                [HttpDelete("deleteBatch/{id}")]
                public async Task<IActionResult> DeleteBatch(int id)
                {

                    var batch = await _uow.BatchRepository.GetBatchByIdAsync(id);

                    //var hasBatches = await _uow.BatchRepository.HasBatches(id);
                    //if (hasBatches) return BadRequest(new { error = $"can not delete due to Referential Integrity" });

                    if (batch == null) return NotFound(new { error = $"batch not found with id {id}" });

                    //_uow.BatchRepository.RemoveBatch(batch);
                    if (!await _uow.SaveChangesAsync()) return BadRequest(new { error = $"Unable to delete batch" });

                    return Ok(new { message = $"batch with {id} deleted" });
                }
        */

        [Authorize(Roles = "Admin")]
        [HttpPost("addFaculty/{batchId}")]
        public async Task<IActionResult> AddFaculty(int batchId, AddFacultyDto addFacultyDto)
        {
            var batch = await _uow.BatchRepository.GetBatchByIdWithUsersAsync(batchId);
            if (batch == null) return NotFound(new { error = $"Batch is not present with id -> {batchId}" });

            foreach (var facultyId in addFacultyDto.Faculties)
            {
                if (!batch.Users.Select(bu => bu.UserId).Contains(facultyId))
                {
                    var user = await _uow.UserRepository.GetUserByUserIdAsync(facultyId);
                    var newBatch = new BatchUser()
                    {
                        Batch = batch,
                        User = user,
                        status = await _uow.StatusRepository.GetStatusByIdAsync(1)
                    };
                    batch.Users.Add(newBatch);
                }
            }
            _uow.BatchRepository.UpdateBatchUser(batch);
            if (!await _uow.SaveChangesAsync()) BadRequest(new { error = $"unable to add faculties" });
            return Ok(new { message = $"faculties are added to batch with id -> {batchId}" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{batchId}/removeFaculty/{id}", Name = "removeFaculty")]
        public async Task<IActionResult> RemoveFaculty(int batchId, string id)
        {
            var batchUser = await _uow.BatchUserRepository.GetBatchUserById(batchId, id);
            if (batchUser == null)
                return BadRequest(new { error = $"batch id ->{batchId} is not associated with faculty id -> {id}" });

            _uow.BatchUserRepository.RemoveBatchUser(batchUser);
            if (!await _uow.SaveChangesAsync())
                return BadRequest(new { error = $"unable to remove faculty from batch" });

            return Ok(new { message = $"faculty id -> {id} removed form batch with id -> {batchId}" });
        }

        [Authorize(Roles = "Faculty")]
        [HttpPost("updateSatchStatus/{batchId}")]
        public async Task<IActionResult> UpdateBatchStatus(UpdateBatchStatusDto updateBatchStatusDto, int batchId)
        {
            var userId = User.GetUserId();
            var batchUser = await _uow.BatchUserRepository.GetBatchUserById(batchId, userId);
            if (batchUser == null)
                return NotFound(new { error = $"batch id ->{batchId} is not associated with faculty id -> {User.GetUsername()}" });

            var status = await _uow.StatusRepository.GetStatusByIdAsync(updateBatchStatusDto.StatusId);
            batchUser.status = status;

            if (!await _uow.SaveChangesAsync())
                return BadRequest(new { error = $"unable to update status" });

            return Ok(new { message = $"batch status update to {status.Name} by {User.GetUsername()}"});
        }
    }
}
