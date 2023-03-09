using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademyPortalAPI.Controllers
{
    [Authorize(Roles = "Admin, Faculty")]
    public class StatusController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;

        public StatusController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("getAllStatus")]
        public async Task<ActionResult<IEnumerable<StatusDto>>> GerRole()
        {
            var roles = await _uow.StatusRepository.GetAllStatusDtos();
            return Ok(roles);
        }
    }
}
