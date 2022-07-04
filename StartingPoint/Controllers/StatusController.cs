using StartingPoint.Data;
using StartingPoint.Models;
using StartingPoint.Models.StatusViewModel;
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
    public class StatusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public StatusController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }
        public async Task<string> GetMaxID()
        { 
            int StatusID = 0;
            var Id = await _context.Statuses.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (Id == null)
            {
                StatusID = 1;
            }
            else
            {
                StatusID = Convert.ToInt32(Id.StatusId.Remove(0, 3)) + 1;
            }
            return "AS-" + StatusID.ToString("000");
        }

        [Authorize(Roles = Pages.MainMenu.Status.RoleName)]
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
                    || obj.StatusId.ToLower().Contains(searchValue)
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

        private IQueryable<StatusGridViewModel> GetGridItem()
        {
            try
            {
                return (from _Status in _context.Statuses
                        where _Status.Cancelled == false
                        select new StatusGridViewModel
                        {
                            Id = _Status.Id,
                            StatusId = _Status.StatusId,
                            Description = _Status.Description,
                            CreatedDate = _Status.CreatedDate,                           
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
            StatusCRUDViewModel vm = await _context.Statuses.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            StatusCRUDViewModel vm = new StatusCRUDViewModel();
            if (id > 0) vm = await _context.Statuses.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (id == 0) { vm.StatusId = await GetMaxID(); }
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(StatusCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = await _context.Statuses.Where(x => x.Description == vm.Description).ToListAsync();
                        if (isCheck.Count() == 0)
                        {
                            Status _Status = new Status();
                            if (vm.Id > 0)
                            {
                                _Status = await _context.Statuses.FindAsync(vm.Id);

                                vm.CreatedDate = _Status.CreatedDate;
                                vm.CreatedBy = _Status.CreatedBy;
                                vm.ModifiedDate = DateTime.Now;
                                vm.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Entry(_Status).CurrentValues.SetValues(vm);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Status Updated Successfully. ID: " + _Status.Id;
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                _Status = vm;
                                _Status.CreatedDate = DateTime.Now;
                                _Status.ModifiedDate = DateTime.Now;
                                _Status.CreatedBy = HttpContext.User.Identity.Name;
                                _Status.ModifiedBy = HttpContext.User.Identity.Name;
                                _context.Add(_Status);
                                await _context.SaveChangesAsync();
                                TempData["successAlert"] = "Status Created Successfully. ID: " + _Status.Id;
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
                var _Status = await _context.Statuses.FindAsync(id);
                _Status.ModifiedDate = DateTime.Now;
                _Status.ModifiedBy = HttpContext.User.Identity.Name;
                _Status.Cancelled = true;

                _context.Update(_Status);
                await _context.SaveChangesAsync();
                return new JsonResult(_Status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.Statuses.Any(e => e.Id == id);
        }
    }
}
