using Microsoft.AspNetCore.Mvc;
using AcademyPortal.ViewModel;
using AcademyPortal.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using AcademyPortal.Extensions;
using Microsoft.AspNetCore.Identity;
using AcademyPortal.Model;
using Microsoft.EntityFrameworkCore;
namespace AcademyPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _uow;

        public UserController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Route("user/signup", Name = "signup")]
        public IActionResult SignUp()
        {
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
                var result = await _uow.UserRepository.CreateUserAsync(signUpUserViewModel);
                if (result.Succeeded)
                {
                    return RedirectToRoute("signIn");
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

        [Route("user/signIn", Name = "signIn")]
        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["Message"] = $"Welcome To Academy Portal";
                return RedirectToAction("Index", "Home");
            }
            return View("SignIn");
        }

        [Route("user/signIn", Name = "signIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _uow.UserRepository.GetUserByUsernameAsync(signInViewModel.Email);     
                if(user == null){
                    ModelState.AddModelError("", $"This Username or Email is not registered.");
                    return View("SignIn");
                }

                if(user?.status?.Name != "Active")
                {
                    ModelState.AddModelError("", $"Hi, {user?.Email} Your Current Status is {user?.status?.Name} please contact Admin");
                    return View("SignIn");
                } 

                var result = await _uow.UserRepository.SignInUserAsync(signInViewModel);
                if (result.Succeeded)
                {
                    TempData["Message"] = $"Welcome To Academy Portal";
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Provided Credentials are Invalid");
            }
            return View("SignIn");
        }

        [Authorize]
        [Route("user/logout", Name = "logout")]
        public async Task<IActionResult> Logout()
        {
            await _uow.UserRepository.LogoutUser();
            return RedirectToAction("signIn");
        }

        [Authorize]
        [Route("user/profile", Name = "profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(User.GetUserId());
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
            var user = await _uow.UserRepository.GetUserByIdAsync(User.GetUserId());
            if (ModelState.IsValid)
            {
                var result = await _uow.UserRepository.UpdateProfileAsync(profileViewModel, user);
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
            var users = await _uow.UserRepository.GetUsersAsync();
            return View("ListUser", users);
        }

        [Authorize(Roles = "Admin")]
        [Route("user/updateRole/{id}", Name = "updateRole")]
        public async Task<IActionResult> UpdateUserRole(string id)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(id);
            ViewBag.listRoles = from role in await _uow.RoleRepository.GetRolesAsync() select role.Name;
            ViewBag.listStatus = from status in await _uow.StatusRepository.GetAllStatus() select status.Name;
            ViewData["user"] = user;
            var roles = (await _uow.UserRepository.GetUsersRolesAsync(user)).ToList();
            var updateRoleViewModel = new UpdateRoleViewModel
            {
                Role = roles.Any() ? roles[0] : "Please Select any role",
                Status = user.status.Name
            };
            return View("UpdateUserRole", updateRoleViewModel);
        }

        [Authorize(Roles = "Admin")]
        [Route("user/updateRole/{Id}", Name = "updateRole")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string id, UpdateRoleViewModel updateRoleViewModel)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    //Remove Previous roles
                    var userRoles = await _uow.UserRepository.GetUsersRolesAsync(user);
                    await _uow.UserRepository.RemoveUserFromRolesAsync(user, userRoles);
            
                    //Add to new role
                    await _uow.UserRepository.AddUserToRoleAsync(user, updateRoleViewModel.Role);
                    TempData["Message"] = $"Role updated to {updateRoleViewModel.Role} successfully !!";
                    TempData["Type"] = "success";
                    return RedirectToRoute("updateRole");
                }
            }
            ViewBag.listRoles = from role in await _uow.RoleRepository.GetRolesAsync() select role.Name;
            ViewBag.listStatus = from status in await _uow.StatusRepository.GetAllStatus() select status.Name;
            ViewData["user"] = user;
            return View("UpdateUserRole", updateRoleViewModel);
        }
        
        [Authorize(Roles = "Admin")]
        [Route("user/updateStatus/{id}", Name = "updateStatus")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string id, UpdateRoleViewModel updateRoleViewModel)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    user.status = await _uow.StatusRepository.GetStatusByNameAsync(updateRoleViewModel.Status);
                    if(await _uow.SaveChangesAsync()){
                        Console.WriteLine("Changes are Saved");                          
            
                        TempData["Message"] = $"Status updated to {updateRoleViewModel.Status} successfully !!";
                        TempData["Type"] = "success";
                        return RedirectToRoute("updateRole", new {Id = user.Id});
                    }
                }
            }
            return RedirectToRoute("updateRole", new {Id = user.Id});
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
