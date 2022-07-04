using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.DesignationViewModel;
using StartingPoint.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace StartingPoint.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class DesignationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public DesignationController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }
        public async Task<string> GetMaxID()
        { 
            int DesignationID = 0;
            var Id = await _context.Designations.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                DesignationID = 1;
            }
            else
            {
                DesignationID = Convert.ToInt32(Id.DesignationId.Remove(0, 3)) + 1;
            }
            return "DE-" + DesignationID.ToString("000");
        }

        [Authorize(Roles = Pages.MainMenu.Designation.RoleName)]
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

                var _GetGridItem = GetGridItem();
                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.DesignationId.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)                   

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private IQueryable<DesignationGridViewModel> GetGridItem()
        {
            try
            {
                return (from _Designation in _context.Designations
                        where _Designation.Cancelled == false
                        select new DesignationGridViewModel
                        {
                            Id = _Designation.Id,
                            DesignationId = _Designation.DesignationId,
                            Description = _Designation.Description,
                            CreatedDate = _Designation.CreatedDate,                           
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            DesignationCRUDViewModel vm = await _context.Designations.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            DesignationCRUDViewModel vm = new DesignationCRUDViewModel();
            if (id > 0) vm = await _context.Designations.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.DesignationId = await GetMaxID(); }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(DesignationCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.Designations.Where(x => x.Description == vm.Description).ToListAsync();
                        if (isCheck.Count() == 0)
                        {
                            Designation _Designation = new Designation();
                            if (vm.Id > 0)
                            {
                                _Designation = await _context.Designations.FindAsync(vm.Id);

                                vm.CreatedDate = _Designation.CreatedDate;
                                vm.CreatedBy = _Designation.CreatedBy;
                                vm.ModifiedDate = DateTime.Now;
                                vm.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Entry(_Designation).CurrentValues.SetValues(vm);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Designation Updated Successfully. ID: " + _Designation.Id;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _Designation = vm;
                                _Designation.CreatedDate = DateTime.Now;
                                _Designation.ModifiedDate = DateTime.Now;
                                _Designation.CreatedBy = HttpContext.User.Identity.Name;
                                _Designation.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Add(_Designation);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Designation Created Successfully. ID: " + _Designation.Id;
                                return RedirectToAction(nameof(Index));
                            }
                        }
                        else
                        {
                            TempData["errorDuplication"] = "Record already Exist.";
                            return View("Index");

                        }
                    }
                    TempData["errorAlert"] = "Operation failed.";
                    return View("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!IsExists(vm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _Designation = await _context.Designations.FindAsync(id);
                _Designation.ModifiedDate = DateTime.Now;
                _Designation.ModifiedBy = HttpContext.User.Identity.Name;
                _Designation.Cancelled = true;

                _context.Update(_Designation);
                await _context.SaveChangesAsync();
                return new JsonResult(_Designation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Designations.Any(e => e.Id == id);
        }
    }
}
