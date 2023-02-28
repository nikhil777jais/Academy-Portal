using System.Security.Claims;
using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortalAPI.Repository.User
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpUserDto signUpUserDto);
        Task<SignInResult> SignInUserAsync(SignInDto signInDto);
        Task LogoutUser();
        Task<IdentityResult> UpdateProfileAsync(ProfileDto profileDto, ApplicationUser user);
        Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> RemoveUserFromRolesAsync(ApplicationUser user, IEnumerable<string> roles);
        Task<IEnumerable<string>> GetUsersRolesAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByClaimsAsync(ClaimsPrincipal claims);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
        Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string role);

        //API Specific methods
        Task<ProfileDto> GetProfileById(string id);
        Task<IEnumerable<ProfileDto>> GetProfiles();
    }
}
