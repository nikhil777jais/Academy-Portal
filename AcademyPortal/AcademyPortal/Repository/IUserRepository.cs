using AcademyPortal.Model;
using AcademyPortal.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortal.Repository
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpUserViewModel signUpUserViewModel);

        Task<SignInResult> SignInUserAsync(SignInViewModel signInViewModel);

        Task LogoutUser();
        Task<IdentityResult> UpdateProfileAsync(ProfileViewModel profileViewModel, ApplicationUser user);
    }
}
