using Microsoft.AspNetCore.Mvc;
using AcademyPortal.ViewModel;
using AcademyPortal.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;
using Microsoft.AspNetCore.Identity;
using AcademyPortal.Model;
using Microsoft.EntityFrameworkCore;
namespace AcademyPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AcademyPortalDbContext _db;

        public UserController(IUserRepository userRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AcademyPortalDbContext db)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        [Route("user/signup", Name = "signup")]
        public async Task<IActionResult> SignUp()
        {
            Console.WriteLine("-------------------");
            Console.WriteLine("Some random new text ");
            Console.WriteLine("-------------------");
            if (User.Identity.IsAuthenticated)
            {
                TempData["Message"] = $"Welcome To Academy Portal";
                return RedirectToAction("Index", "Home");
            }
            return View("SignUp");
        }

        [Route("user/signup", Name = "signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpUserViewModel signUpUserViewModel)
        {
            
            if (ModelState.IsValid)
            {
                var result = await _userRepository.CreateUserAsync(signUpUserViewModel);
                if (result.Succeeded)
                {
                    return RedirectToRoute("signin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View("SignUp");
        }

        [Route("user/signin", Name = "signin")]
        public async Task<IActionResult> SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["Message"] = $"Welcome To Academy Poertal";
                return RedirectToAction("Index", "Home");
            }
            return View("SignIn");
        }

        [Route("user/signin", Name = "signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
        {
            var user = await _userManager.Users.Include(u => u.status).Where(u => u.Email == signInViewModel.Email).FirstOrDefaultAsync();
            if(user.status.Name != "Active")
            {
                ModelState.AddModelError("", $"Hi, {user.Email} Your Current Status is {user.status.Name}");
                return View("SignIn");
            }            
            if (ModelState.IsValid)
            {
                var result = await _userRepository.SignInUserAsync(signInViewModel);
                if (result.Succeeded)
                {
                    TempData["Message"] = $"Welcome To Academy Poertal";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Provided Credentials are Invalid");
                }
            }
            return View("SignIn");
        }

        [Authorize]
        [Route("user/logout", Name = "logout")]
        public async Task<IActionResult> Logout()
        {
            await _userRepository.LogoutUser();
            return RedirectToAction("signin");
        }

        [Authorize]
        [Route("user/profile", Name = "profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _userManager.Users.Include(x => x.status).FirstOrDefault(x => x.Id == userId);
            var profileViewModel = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender
            };
            
            ViewData["user"] = user;
            return View("Profile", profileViewModel);
        }
        
        [Authorize]
        [Route("user/profile", Name = "profile")]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel profileViewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _userManager.Users.Include(x => x.status).FirstOrDefault(x => x.Id == userId);
            if (ModelState.IsValid)
            {
                var result = await _userRepository.UpdateProfileAsync(profileViewModel, user);
                if (result.Succeeded)
                {
                    ViewData["user"] = user;
                    TempData["Message"] = $"Profile Updated Successfully !!";
                    return View("Profile", profileViewModel);
                }
            }
            ViewData["user"] = user;
            return View("Profile", profileViewModel);
        }

        [Authorize(Roles = "Admin")]
        [Route("user/list", Name = "list")]
        public async Task<IActionResult> ListUser()
        {
            var users = _userManager.Users.Include(u => u.status).ToList();
            return View("ListUser", users);
        }

        [Authorize(Roles = "Admin")]
        [Route("user/updaterole/{Id}", Name = "updaterole")]
        public async Task<IActionResult> UpdateUserRole(string Id)
        {
            var user = _userManager.Users.Include(u => u.status).FirstOrDefault(u => u.Id == Id);
            ViewBag.lstroles = (from role in _roleManager.Roles select role.Name).ToList();
            ViewBag.lststatus = await (from status in _db.AllStatus select status.Name).ToListAsync();
            ViewData["user"] = user;
            var roles = await _userManager.GetRolesAsync(user);
            var updateRoleViewModel = new UpdateRoleViewModel
            {
                Role = roles.Any() ? roles[0] : "Please Select any role",
                Status = user.status.Name
            };
            return View("UpdateUserRole", updateRoleViewModel);
        }

        [Authorize(Roles = "Admin")]
        [Route("user/updaterole/{Id}", Name = "updaterole")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string Id, UpdateRoleViewModel updateRoleViewModel)
        {
            var user = _userManager.Users.Include(u => u.status).FirstOrDefault(u => u.Id == Id);
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    //Remove Privious roles
                    var useroles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, useroles);
                    
                    //Add to new role
                    await _userManager.AddToRoleAsync(user, updateRoleViewModel.Role);
                    TempData["Message"] = $"Role updated to {updateRoleViewModel.Role} successfully !!";
                    TempData["Type"] = "success";
                    return RedirectToRoute("updaterole");
                }
            }
            ViewBag.lstroles = (from role in _roleManager.Roles select role.Name).ToList();
            ViewBag.lststatus = await (from status in _db.AllStatus select status.Name).ToListAsync();
            ViewData["user"] = user;
            return View("UpdateUserRole", updateRoleViewModel);
        }
        
        [Authorize(Roles = "Admin")]
        [Route("user/updatestatus/{Id}", Name = "updatestatus")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string Id, UpdateRoleViewModel updateRoleViewModel)
        {
            var user = _userManager.Users.Include(u => u.status).FirstOrDefault(u => u.Id == Id);
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    user.status = _db.AllStatus.Where(s => s.Name == updateRoleViewModel.Status).FirstOrDefault();

                    await _userManager.UpdateAsync(user);
                    TempData["Message"] = $"Status updated to {updateRoleViewModel.Status} successfully !!";
                    TempData["Type"] = "success";
                    return RedirectToRoute("updaterole", new {Id = user.Id});
                }
            }
            return RedirectToRoute("updaterole", new {Id = user.Id});
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
