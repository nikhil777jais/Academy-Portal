using System.Security.Claims;
using AcademyPortal.Model;
using AcademyPortal.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortal.Repository.User
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpUserViewModel signUpUserViewModel);
        Task<SignInResult> SignInUserAsync(SignInViewModel signInViewModel);
        Task LogoutUser();
        Task<IdentityResult> UpdateProfileAsync(ProfileViewModel profileViewModel, ApplicationUser user);
        
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByClaimsAsync(ClaimsPrincipal claims);
        Task<IEnumerable<ApplicationUser>> GetUsersAsync();
    }
}
