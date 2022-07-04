using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StartingPoint.Models;
using StartingPoint.Models.UserAccountViewModel;

namespace StartingPoint.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class UserRoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = Pages.MainMenu.UserManagement.RoleName)]
        public async Task<IActionResult> Index(int id)
        {
            //UserProfileViewModel _UserProfileViewModel = _iAccountUserRepository.Get(id);
            var _ApplicationUser = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
            ViewBag.ApplicationUserId = _ApplicationUser.Id;
            var user = await _userManager.FindByIdAsync(_ApplicationUser.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {_ApplicationUser.Id} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model.OrderBy(x => x.RoleName).ToList());
        }


        [HttpPost]
        public async Task<IActionResult> ManageRole(List<ManageUserRolesViewModel> model)
        {
            string _ApplicationUserId = TempData["ApplicationUserId"].ToString();
            var user = await _userManager.FindByIdAsync(_ApplicationUserId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            TempData["successAlert"] = "Role update Successfully. User Name: " + user.Email;
            return RedirectToAction(nameof(Index));
        }
    }
}