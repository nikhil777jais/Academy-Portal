using Microsoft.AspNetCore.Mvc;
using AcademyPortal.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using AcademyPortal.Extensions;
using AcademyPortal.DTOs;

namespace AcademyPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _uow;

        public UserController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Route("user/signUp", Name = "signUp")]
        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                TempData["Message"] = $"Welcome To Academy Portal";
                return RedirectToAction("Index", "Home");
            }
            return View("SignUp");
        }

        [Route("user/signUp", Name = "signUp")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpUserDto signUpUserDto)
        {
            
            if (ModelState.IsValid)
            {
                var result = await _uow.UserRepository.CreateUserAsync(signUpUserDto);
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
        public async Task<IActionResult> SignIn(SignInDto signInDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _uow.UserRepository.GetUserByUsernameAsync(signInDto.Email);     
                if(user == null){
                    ModelState.AddModelError("", $"This Username or Email is not registered.");
                    return View("SignIn");
                }

                if(user?.status?.Name != "Active")
                {
                    ModelState.AddModelError("", $"Hi, {user?.Email} Your Current Status is {user?.status?.Name} please contact Admin");
                    return View("SignIn");
                } 

                var result = await _uow.UserRepository.SignInUserAsync(signInDto);
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
            var profileDto = new ProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender
            };
            
            ViewData["user"] = user;
            return View("Profile", profileDto);
        }
        
        [Authorize]
        [Route("user/profile", Name = "profile")]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileDto profileDto)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(User.GetUserId());
            if (ModelState.IsValid)
            {
                var result = await _uow.UserRepository.UpdateProfileAsync(profileDto, user);
                if (result.Succeeded)
                {
                    ViewData["user"] = user;
                    TempData["Message"] = $"Profile Updated Successfully !!";
                    return View("Profile", profileDto);
                }
            }
            ViewData["user"] = user;
            return View("Profile", profileDto);
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
            var updateRoleDto = new UpdateRoleDto
            {
                Role = roles.Any() ? roles[0] : "Please Select any role",
                Status = user.status.Name
            };
            return View("UpdateUserRole", updateRoleDto);
        }

        [Authorize(Roles = "Admin")]
        [Route("user/updateRole/{Id}", Name = "updateRole")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string id, UpdateRoleDto updateRoleDto)
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
                    await _uow.UserRepository.AddUserToRoleAsync(user, updateRoleDto.Role);
                    TempData["Message"] = $"Role updated to {updateRoleDto.Role} successfully !!";
                    TempData["Type"] = "success";
                    return RedirectToRoute("updateRole");
                }
            }
            ViewBag.listRoles = from role in await _uow.RoleRepository.GetRolesAsync() select role.Name;
            ViewBag.listStatus = from status in await _uow.StatusRepository.GetAllStatus() select status.Name;
            ViewData["user"] = user;
            return View("UpdateUserRole", updateRoleDto);
        }
        
        [Authorize(Roles = "Admin")]
        [Route("user/updateStatus/{id}", Name = "updateStatus")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string id, UpdateRoleDto updateRoleDto)
        {
            var user = await _uow.UserRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    user.status = await _uow.StatusRepository.GetStatusByNameAsync(updateRoleDto.Status);
                    if(await _uow.SaveChangesAsync()){
                        Console.WriteLine("Changes are Saved");                          
            
                        TempData["Message"] = $"Status updated to {updateRoleDto.Status} successfully !!";
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
