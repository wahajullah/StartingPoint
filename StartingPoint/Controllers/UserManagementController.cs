using StartingPoint.Data;
using StartingPoint.Helpers;
using StartingPoint.Models;
using StartingPoint.Models.UserAccountViewModel;
using StartingPoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace StartingPoint.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class UserManagementController : Controller
    {
        private readonly IRoles _roles;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICommon _iCommon;

        public UserManagementController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            IRoles roles,
            ICommon iCommon)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _roles = roles;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.MainMenu.UserManagement.RoleName)]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetDataTabelData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                var _AccountUser = _context.UserProfile.Where(x => x.Cancelled == false);
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _AccountUser = _context.UserProfile.Where(x => x.Cancelled == false).OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _AccountUser = _AccountUser.Where(obj => obj.FirstName.ToLower().Contains(searchValue)
                    || obj.LastName.ToLower().Contains(searchValue)
                    || obj.PhoneNumber.ToLower().Contains(searchValue)
                    || obj.Email.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _AccountUser.Count();

                var result = _AccountUser.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ViewUserDetails(Int64 id)
        {
            var _GetByUserProfileInfo = _iCommon.GetByUserProfileInfo(id);
            return PartialView("_ViewUserDetails", _GetByUserProfileInfo);
        }

        public IActionResult AddEditUserAccount(Int64 id)
        {
            UserProfileViewModel _UserProfileViewModel = new UserProfileViewModel();
            if (id > 0) _UserProfileViewModel = _iCommon.GetByUserProfile(id);
            return PartialView("_AddEditUserAccount", _UserProfileViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditUserAccount(UserProfileViewModel _UserProfileViewModel)
        {
            try
            {
                if (_UserProfileViewModel.UserProfileId > 0)
                {
                    UserProfile _UserProfile = _iCommon.GetByUserProfile(_UserProfileViewModel.UserProfileId);
                    _UserProfile.FirstName = _UserProfileViewModel.FirstName;
                    _UserProfile.LastName = _UserProfileViewModel.LastName;
                    _UserProfile.PhoneNumber = _UserProfileViewModel.PhoneNumber;
                    _UserProfile.Address = _UserProfileViewModel.Address;
                    _UserProfile.Country = _UserProfileViewModel.Country;

                    if (_UserProfileViewModel.ProfilePicture != null)
                        _UserProfile.ProfilePicture = "/upload/" + _iCommon.UploadedFile(_UserProfileViewModel.ProfilePicture);

                    _UserProfile.ModifiedDate = DateTime.Now;
                    _UserProfile.ModifiedBy = HttpContext.User.Identity.Name;
                    var result2 = _context.UserProfile.Update(_UserProfile);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "User info Updated Successfully. User Name: " + _UserProfile.Email;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    await _roles.GenerateRolesFromPagesAsync();
                    ApplicationUser _ApplicationUser = new ApplicationUser()
                    {
                        UserName = _UserProfileViewModel.Email,
                        PhoneNumber = _UserProfileViewModel.PhoneNumber,
                        PhoneNumberConfirmed = true,
                        Email = _UserProfileViewModel.Email,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(_ApplicationUser, _UserProfileViewModel.PasswordHash);
                    if (result.Succeeded)
                    {
                        _UserProfileViewModel.PasswordHash = _ApplicationUser.PasswordHash;
                        _UserProfileViewModel.ConfirmPassword = _ApplicationUser.PasswordHash;
                        _UserProfileViewModel.ApplicationUserId = _ApplicationUser.Id;

                        _UserProfileViewModel.CreatedDate = DateTime.Now;
                        _UserProfileViewModel.ModifiedDate = DateTime.Now;
                        _UserProfileViewModel.CreatedBy = HttpContext.User.Identity.Name;
                        _UserProfileViewModel.ModifiedBy = HttpContext.User.Identity.Name;


                        UserProfile _UserProfile = new UserProfile();
                        _UserProfile = _UserProfileViewModel;

                        if (_UserProfileViewModel.ProfilePicture != null)
                            _UserProfile.ProfilePicture = "/upload/" + _iCommon.UploadedFile(_UserProfileViewModel.ProfilePicture);
                        else
                            _UserProfile.ProfilePicture = "/upload/" + "blank-person.png";

                        var result2 = await _context.UserProfile.AddAsync(_UserProfile);
                        await _context.SaveChangesAsync();
                        for (int i = 0; i < DefaultUserPage.PageCollection.Length; i++)
                        {
                            await _userManager.AddToRoleAsync(_ApplicationUser, DefaultUserPage.PageCollection[i]);
                        }

                        var _AlertMessage = "User Created Successfully. User Name: " + _ApplicationUser.Email;
                        return new JsonResult(_AlertMessage);
                    }
                    else
                    {
                        string errorMessage = string.Empty;
                        foreach (var item in result.Errors)
                        {
                            errorMessage = errorMessage + " " + item.Description;
                        }
                        var _AlertMessage = "error Account creation failed." + errorMessage;
                        return new JsonResult(_AlertMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("error" + ex.Message);
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordAdmin(Int64 id)
        {
            UserProfile _UserProfile = _iCommon.GetByUserProfile(id);
            var _ApplicationUser = await _userManager.FindByIdAsync(_UserProfile.ApplicationUserId);
            ResetPasswordViewModel _ResetPasswordViewModel = new ResetPasswordViewModel();
            _ResetPasswordViewModel.ApplicationUserId = _ApplicationUser.Id;
            return PartialView("_ResetPasswordAdmin", _ResetPasswordViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordAdmin(ResetPasswordViewModel vm)
        {
            try
            {
                string AlertMessage = string.Empty;
                var _ApplicationUser = await _userManager.FindByIdAsync(vm.ApplicationUserId);
                if (vm.NewPassword.Equals(vm.ConfirmPassword))
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(_ApplicationUser);
                    var _ResetPasswordAsync = await _userManager.ResetPasswordAsync(_ApplicationUser, code, vm.NewPassword);
                    if (_ResetPasswordAsync.Succeeded)
                        AlertMessage = "Reset Password Succeeded. User name: " + _ApplicationUser.Email;
                    else
                    {
                        string errorMessage = string.Empty;
                        foreach (var item in _ResetPasswordAsync.Errors)
                        {
                            errorMessage = errorMessage + " " + item.Description;
                        }
                        AlertMessage = "error Reset password failed." + errorMessage;
                    }
                }
                return new JsonResult(AlertMessage);
            }
            catch (Exception ex)
            {
                return new JsonResult("error" + ex.Message);
                throw ex;
            }            
        }

        public async Task<IActionResult> ManageRole(int id)
        {
            await _roles.GenerateRolesFromPagesAsync();
            UserProfile _UserProfile = _iCommon.GetByUserProfile(id);
            ViewBag.ApplicationUserId = _UserProfile.ApplicationUserId;
            var user = await _userManager.FindByIdAsync(_UserProfile.ApplicationUserId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {_UserProfile.ApplicationUserId} cannot be found";
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
            return PartialView("_ManageRole", model.OrderBy(x => x.RoleName).ToList());
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

        [HttpPost]
        public async Task<IActionResult> DeleteUserAccount(int id)
        {
            UserProfile _UserProfile = _iCommon.GetByUserProfile(id);
            var _ApplicationUser = await _userManager.FindByIdAsync(_UserProfile.ApplicationUserId);
            var _DeleteAsync = await _userManager.DeleteAsync(_ApplicationUser);

            if (_DeleteAsync.Succeeded)
            {
                _UserProfile.Cancelled = true;
                _UserProfile.ModifiedDate = DateTime.Now;
                _UserProfile.ModifiedBy = HttpContext.User.Identity.Name;
                var result2 = _context.UserProfile.Update(_UserProfile);
                await _context.SaveChangesAsync();
            }
            return new JsonResult(_UserProfile);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

    }
}
