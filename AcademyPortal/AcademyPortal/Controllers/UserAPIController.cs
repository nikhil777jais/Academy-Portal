﻿using AcademyPortal.DTOs;
using AcademyPortal.Extensions;
using AcademyPortal.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AcademyPortal.Controllers
{

    public class UserAPIController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;

        public UserAPIController(IUnitOfWork uow)
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
                Token = await _uow.TokenService.CreateToken(user)
            };
            return Ok(userDto);
        }
        
        [HttpPost("signIn")]
        public async Task<ActionResult<UserDto>> SignIn(SignInDto signInDto)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(signInDto.Email);

            if (user == null) return Unauthorized(new { errors = $"{signInDto.Email} is not registered" });

            if(user.status.Name != "Active") return Unauthorized(new { errors = $"Please contact admin, Status is not active"});

            var result = await _uow.UserRepository.SignInUserAsync(signInDto);

            if (!result.Succeeded) return Unauthorized(result);
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Gender = user.Gender,
                Token = await _uow.TokenService.CreateToken(user)
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
        public async Task<ActionResult<ProfileDto>> UpdateRole(string id, UpdateRole1Dto updateRole1Dto)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(id);
            //Remove Previous roles
            var userRoles = await _uow.UserRepository.GetUsersRolesAsync(user);
            var result = await _uow.UserRepository.RemoveUserFromRolesAsync(user, userRoles);
            if(!result.Succeeded) return BadRequest(new { errors = $"unable to remove existing roles" });

            //Add to new role
            result = await _uow.UserRepository.AddUserToRoleAsync(user, updateRole1Dto.Role);
            if (!result.Succeeded) return BadRequest(new { errors = $"unable to add in role" });

            return Ok($"Added in role {updateRole1Dto.Role} success");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("updateStatus/{id}")]
        public async Task<ActionResult<ProfileDto>> UpdateStatus(string id, UpdateStatusDto updateStatusDto)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(id);
            var status = await _uow.StatusRepository.GetStatusByNameAsync(updateStatusDto.Status);
            if (status == null) return BadRequest(new { errors = $"Status not found" });

            user.status = status;
            if (!(await _uow.SaveChangesAsync()))
            {
                return BadRequest(new { error = $"unable to update Status"});
            }
            return Ok($"User status updated to {updateStatusDto.Status} success");
        }






    }
}
