using AcademyPortal.Model;
using AcademyPortal.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace AcademyPortal.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private readonly AcademyPortalDbContext _db;

        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager, AcademyPortalDbContext db)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _db = db;
        }

        public async Task<IdentityResult> CreateUserAsync(SignUpUserViewModel signUpUserViewModel)
        {
            var user = new ApplicationUser()
            {
                Email = signUpUserViewModel.Email,
                UserName = signUpUserViewModel.Email,
                status = await _db.AllStatus.FindAsync(1)
            };

            var result = await _userManager.CreateAsync(user, signUpUserViewModel.Password);
            return result;
        }

        public async Task<SignInResult> SignInUserAsync(SignInViewModel signInViewModel)
        {
            var result = await _singInManager.PasswordSignInAsync(signInViewModel.Email, signInViewModel.Password, false, false);
            return result;
        }

        public async Task LogoutUser()
        {
            await _singInManager.SignOutAsync();
        }
    
        public async Task<IdentityResult> UpdateProfileAsync(ProfileViewModel profileViewModel, ApplicationUser applicationUser)
        {
            applicationUser.FirstName = profileViewModel.FirstName;
            applicationUser.LastName = profileViewModel.LastName;
            applicationUser.PhoneNumber = profileViewModel.PhoneNumber;
            applicationUser.DateOfBirth = profileViewModel.DateOfBirth;
            applicationUser.DateOfBirth = profileViewModel.DateOfBirth;
            applicationUser.Gender = profileViewModel.Gender;

            var result = await _userManager.UpdateAsync(applicationUser);

            return result;
        } 
    }
}
