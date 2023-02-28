using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Extensions;
using AcademyPortalAPI.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AcademyPortalAPI.Controllers
{

    public class UserController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;

        public UserController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("signUp")]
        public async Task<ActionResult> SignUp(SignUpUserDto signUpUserDto)
        {
            var result = await _uow.UserRepository.CreateUserAsync(signUpUserDto);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var user = await _uow.UserRepository.GetUserByUsernameAsync(signUpUserDto.Email);

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Gender = user.Gender,
                Token = await _uow.TokenService.CreateToken(user),
                Role = user.UserRoles.FirstOrDefault().ApplicationRole.Name,
                Status = user.status.Name,
                ExpiresIn = "3600"
            };
            return Ok(userDto);
        }

        [HttpPost("signIn")]
        public async Task<ActionResult<UserDto>> SignIn(SignInDto signInDto)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(signInDto.Email);

            if (user == null) return Unauthorized(new { errors = $"{signInDto.Email} is not registered" });

            if (user.status.Name != "Active") return Unauthorized(new { errors = $"Please contact admin, Status is not active" });

            var result = await _uow.UserRepository.SignInUserAsync(signInDto);

            if (!result.Succeeded) return Unauthorized(result);
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Gender = user.Gender,
                Token = await _uow.TokenService.CreateToken(user),
                Role = user.UserRoles.FirstOrDefault().ApplicationRole.Name,
                Status = user.status.Name,
                ExpiresIn = "3600"
            };
            return Ok(userDto);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<ProfileDto>> Profile()
        {
            var profile = await _uow.UserRepository.GetProfileById(User.GetUserId());
            if (profile == null) return NotFound();

            return Ok(profile);
        }

        [Authorize]
        [HttpPatch("profile")]
        public async Task<ActionResult> Profile(ProfileDto profileDto)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(User.GetUserId());
            if (user == null) return NotFound();

            var result = await _uow.UserRepository.UpdateProfileAsync(profileDto, user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { message = $"Profile updated successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getUsers")]
        public async Task<ActionResult<IList<ProfileDto>>> GetUsers()
        {
            var profiles = await _uow.UserRepository.GetProfiles();
            if (profiles == null) return NotFound();

            return Ok(profiles);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getUser/{id}")]
        public async Task<ActionResult<ProfileDto>> GetUserById(string id)
        {
            var profile = await _uow.UserRepository.GetProfileById(id);
            if (profile == null) return NotFound();

            return Ok(profile);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("updateRole/{id}")]
        public async Task<ActionResult<ProfileDto>> UpdateRole(string id, RoleDto roleDto)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(id);
            //Remove Previous roles
            var userRoles = await _uow.UserRepository.GetUsersRolesAsync(user);
            var result = await _uow.UserRepository.RemoveUserFromRolesAsync(user, userRoles);
            if (!result.Succeeded) return BadRequest(new { errors = $"unable to remove existing roles" });

            //Add to new role
            result = await _uow.UserRepository.AddUserToRoleAsync(user, roleDto.Name);
            if (!result.Succeeded) return BadRequest(new { errors = $"unable to add in role" });

            return Ok(new { message = $"Added in role {roleDto.Name} success" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("updateStatus/{id}")]
        public async Task<ActionResult<ProfileDto>> UpdateStatus(string id, StatusDto statusDto)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(id);

            if (user.status.Name == statusDto.Name)
            {
                return Ok(new { message = $"User status updated to {statusDto.Name} success" });
            }

            var status = await _uow.StatusRepository.GetStatusByNameAsync(statusDto.Name);
            if (status == null) return BadRequest(new { errors = $"Status not found" });

            user.status = status;
            if (!await _uow.SaveChangesAsync())
            {
                return BadRequest(new { error = $"unable to update Status" });
            }
            return Ok(new { message = $"User status updated to {statusDto.Name} success" });
        }

    }
}
