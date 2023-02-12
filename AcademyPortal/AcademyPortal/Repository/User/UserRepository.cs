using System.Security.Claims;
using AcademyPortal.Models;
using AcademyPortal.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcademyPortal.Repository.User
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

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _db.Users.Include(u => u.status).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _db.Users.Include(u => u.status).SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<ApplicationUser> GetUserByClaimsAsync(ClaimsPrincipal claims)
        {
            return await _userManager.GetUserAsync(claims);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await _db.Users.Include(u => u.status).ToListAsync();   
        }

        public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveUserFromRolesAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task<IEnumerable<string>> GetUsersRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string  role)
        {
            return await _userManager.GetUsersInRoleAsync(role);
        }
    }
}
