using System.Security.Claims;
using AcademyPortal.DTOs;
using AcademyPortal.Models;
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

        public async Task<IdentityResult> CreateUserAsync(SignUpUserDto signUpUserDto)
        {
            var user = new ApplicationUser()
            {
                Email = signUpUserDto.Email,
                UserName = signUpUserDto.Email,
                status = await _db.AllStatus.FindAsync(1)
            };

            var result = await _userManager.CreateAsync(user, signUpUserDto.Password);
            return result;
        }

        public async Task<SignInResult> SignInUserAsync(SignInDto signInDto)
        {
            var result = await _singInManager.PasswordSignInAsync(signInDto.Email, signInDto.Password, false, false);
            return result;
        }

        public async Task LogoutUser()
        {
            await _singInManager.SignOutAsync();
        }
    
        public async Task<IdentityResult> UpdateProfileAsync(ProfileDto profileDto, ApplicationUser applicationUser)
        {
            applicationUser.FirstName = profileDto.FirstName;
            applicationUser.LastName = profileDto.LastName;
            applicationUser.PhoneNumber = profileDto.PhoneNumber;
            applicationUser.DateOfBirth = profileDto.DateOfBirth;
            applicationUser.DateOfBirth = profileDto.DateOfBirth;
            applicationUser.Gender = profileDto.Gender;

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
